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
    public class MeasurementController : ControllerBase
    {
        IMongoService db;
        public MeasurementController(IMongoService _db)
        {
            db = _db;
        }

    }
}