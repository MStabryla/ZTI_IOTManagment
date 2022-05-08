using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SysOT.Models;
using SysOT.Services;
//using backend.Models;

namespace SysOT.Controllers
{
    [Route("api/[controller]")]
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
            // TODO: Your code here
            await Task.Yield();

            return new List<Device> { };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetDeviceById(int id)
        {
            // TODO: Your code here
            await Task.Yield();

            return null;
        }

        [HttpPost("")]
        public async Task<ActionResult<Device>> PostDevice(Device model)
        {
            // TODO: Your code here
            await Task.Yield();

            return null;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice(int id, Device model)
        {
            // TODO: Your code here
            await Task.Yield();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Device>> DeleteDeviceById(int id)
        {
            // TODO: Your code here
            await Task.Yield();

            return null;
        }
    }
}