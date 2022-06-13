using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SysOT.Models;
using SysOT.Services;
//using backend.Models;

namespace SysOT.Controllers
{
    [Route("devices")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        IMongoService db;
        public DevicesController(IMongoService _db)
        {
            db = _db;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            var devices = await db.GetDocumentsAsync<Device>("Devices",(new {}).ToBsonDocument() );
            return Ok(devices.Take(25).ToArray());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetDeviceById(int id)
        {
            var device = await db.GetDocumentsAsync<Device>("Devices",(new {_id = id}).ToBsonDocument() );
            if(device.Count() < 1)
                return NotFound();
            return Ok(device.First());
        }

        [HttpPost("")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<Device>> PostDevice(Device model)
        {
            model.Id = null;
            if(model.Managers == null)
                model.Managers = new string[] { User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value };
            await db.InsertDocumentAsync<Device>("Devices",model);
            return Ok(model);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PutDevice(string id, Device model)
        {
            var devices = await db.GetDocumentsAsync<Device>("Devices",(new {_id = new ObjectId(id)}).ToBsonDocument() );
            if(devices.Count() < 1)
                return NotFound();
            var device = devices.First();
            if(!User.IsInRole("Admin") && !device.Managers.Contains(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value))
                return Forbid();
            var result = await db.UpdateDocuments<Device>("Devices",x => x.Id == id,model);
            if(result != 1)
                throw new Exception("Query didn't affect record!");
            model = (await db.GetDocumentsAsync<Device>("Devices",(new {_id = new ObjectId(id)}).ToBsonDocument())).First();
            return Ok(model);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<Device>> DeleteDeviceById(string id)
        {
            var devices = await db.GetDocumentsAsync<Device>("Devices",(new {_id = new ObjectId(id)}).ToBsonDocument() );
            if(devices.Count() < 1)
                return NotFound();
            var device = devices.First();
            if(!User.IsInRole("Admin") && !device.Managers.Contains(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value))
                return Forbid();
            var result = await db.RemoveDocuments<Device>("Devices",x => x.Id == id);
            if(result != 1)
                throw new Exception("Query didn't affect record!");
            return Ok();
        }
    }
}