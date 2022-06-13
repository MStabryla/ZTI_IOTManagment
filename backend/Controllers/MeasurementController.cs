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
    [ApiController]
    public class MeasurementController : ControllerBase
    {
        IMongoService db;
        public MeasurementController(IMongoService _db)
        {
            db = _db;
        }


        [HttpPost("")]
        [Authorize(Roles = "Device")]
        public async Task<IActionResult> InsertData(MeasurementModel model){
            string userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var devices = await db.GetDocumentsAsync<Device>("Devices",x => x.Managers.Any(y => y == userId));
            if(devices.Count() < 1)
                return NotFound();
            var device = devices.First();
            var measurements = await db.GetDocumentsAsync<MeasurementBucket>("MeasurementBuckets",x => x.StartDate < model.Time && x.EndDate > model.Time && x.MeasurementType.Equals(model.Type) && x.Device.Equals(device));
            if(measurements.Count() < 1)
            {
                var startDate = model.Time.AddDays(-1 * (int)model.Time.DayOfWeek);
                var endDate = model.Time.AddDays(1 * (7 - (int)model.Time.DayOfWeek));
                var measurementBucket = new MeasurementBucket(){
                    StartDate = startDate,EndDate = endDate, MeasurementType = model.Type, Device = device, Values = new List<Measurement<object>>()
                };
                measurementBucket.Values.Add(new Measurement<object>(){ Value = model.Value, Time = model.Time});
                await db.InsertDocumentAsync<MeasurementBucket>("MeasurementBuckets",measurementBucket);
                return Ok(measurementBucket);
            }
            else
            {
                var actBucket = measurements.First();
                actBucket.Values.Add(new Measurement<object>(){Value = model.Value, Time = model.Time});
                await db.UpdateDocuments<MeasurementBucket>("MeasurementBuckets",x => x.Id == actBucket.Id,actBucket);
                return Ok(actBucket);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetData(string id){
            string userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var devices = await db.GetDocumentsAsync<Device>("Devices",x => x.Managers.Any(y => y == userId) && x.Id == id);
            if(devices.Count() < 1)
                return NotFound();
            var device = devices.First();
            var measurementsBucket = await db.GetDocumentsAsync<MeasurementBucket>("MeasurementBuckets",x => x.Device.Id == device.Id);
            //var measurementsBucket = await db.GetDocumentsAsync<MeasurementBucket>("MeasurementBuckets",(new {}).ToBsonDocument());
            return Ok(measurementsBucket);
        }

        [HttpGet("types")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<MeasurementType>>> GetMeasurementTypes()
        {
            var types = await db.GetDocumentsAsync<MeasurementType>("MeasurementTypes",new BsonDocument());
            return Ok(types);
        }

        [HttpGet("types/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<MeasurementType>> GetMeasurementTypeById(string id)
        {
            var type = await db.GetDocumentsAsync<MeasurementType>("MeasurementTypes",x => x.Id == id);
            return Ok(type);
        }

        [HttpPost("types")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<MeasurementType>> PostMeasurementType(MeasurementType model)
        {
            await db.InsertDocumentAsync("MeasurementType",model);
            return Ok(model);
        }

        [HttpPut("types/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PutMeasurementType(string id, MeasurementType model)
        {
            var types = await db.GetDocumentsAsync<MeasurementType>("MeasurementType",x => x.Id == id);
            if(types.Count() < 1)
                return NotFound();
            await db.UpdateDocuments<MeasurementType>("MeasurementType",x => x.Id == id,model);
            var type = (await db.GetDocumentsAsync<MeasurementType>("MeasurementType",x => x.Id == id)).First();
            return Ok(type);
        }

        [HttpDelete("types/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MeasurementType>> DeleteMeasurementTypeById(string id)
        {
            var result = await db.RemoveDocuments<MeasurementType>("MeasurementType",x => x.Id == id);
            if(result != 1)
                throw new Exception("Query didn't affect record!");
            return Ok();
        }
    }
}