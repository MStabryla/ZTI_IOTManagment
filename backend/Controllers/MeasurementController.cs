using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SysOT.Models;
using SysOT.Services;
//using backend.Models;

namespace SysOT.Controllers
{
    [Route("data")]
    [Authorize(Roles = "Device")]
    [ApiController]
    public class MeasurementController : ControllerBase
    {
        IMongoService db;
        public MeasurementController(IMongoService _db)
        {
            db = _db;
        }

        public async Task<IActionResult> InsertData(MeasurementModel model){
            string userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var devices = await db.GetDocumentsAsync<Device>("Devices",x => x.Managers.Any(y => y == userId));
            if(devices.Count() < 1)
                return NotFound();
            var device = devices.First();
            var measurements = await db.GetDocumentsAsync<MeasurementBucket>("Measurements",x => x.StartDate < model.Time && x.EndDate > model.Time && x.MeasurementType.Equals(model.Type) && x.Device.Equals(device));
            if(measurements.Count() < 1)
            {
                var startDate = model.Time.AddDays(-1 * (int)model.Time.DayOfWeek);
                var endDate = model.Time.AddDays(1 * (7 - (int)model.Time.DayOfWeek));
                var measurementBucket = new MeasurementBucket(){
                    StartDate = startDate,EndDate = endDate, MeasurementType = model.Type, Device = device, Values = new List<Measurement<object>>()
                };
                measurementBucket.Values.Add(new Measurement<object>(){ Value = model.Value, Time = model.Time});
                await db.InsertDocumentAsync<MeasurementBucket>("Measurements",measurementBucket);
                return Ok(measurementBucket);
            }
            else
            {
                var actBucket = measurements.First();
                actBucket.Values.Add(new Measurement<object>(){Value = model.Value, Time = model.Time});
                await db.UpdateDocuments<MeasurementBucket>("Measurements",x => x.Id == actBucket.Id,actBucket);
                return Ok(actBucket);
            }
        }
    }
}