using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SysOT.Models;
using SysOT.Services;
//using backend.Models;

namespace SysOT.Controllers
{
    [Route("user")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IMongoService db;
        public UserController(IMongoService _db)
        {
            db = _db;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await db.GetDocumentsAsync<User>("Users",(new {}).ToBsonDocument() );
            return Ok(users.Take(25).ToArray());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var users = await db.GetDocumentsAsync<User>("Users",(new {_id = id}).ToBsonDocument() );
            if(users.Count() < 1)
                return NotFound();
            return Ok(users.First());
        }

        [HttpPost("")]
        public async Task<IActionResult> PostUser(User model)
        {
            await db.InsertDocumentAsync<User>("Users",model);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User model)
        {
            var users = await db.GetDocumentsAsync<User>("Users",(new {_id = new ObjectId(id)}).ToBsonDocument() );
            if(users.Count() < 1)
                return NotFound();
            var result = await db.UpdateDocuments<User>("Users",x => x.Id == id,model);
            if(result != 1)
                throw new Exception("Wrong query to database");
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            var users = await db.GetDocumentsAsync<User>("Users",(new {_id = new ObjectId(id)}).ToBsonDocument() );
            if(users.ToList().Count() < 1)
                return NotFound();
            var result = await db.RemoveDocuments<User>("Users",x => x.Id == id);
            if(result != 1)
                throw new Exception("Wrong query to database");
            return Ok();
        }
    }
}