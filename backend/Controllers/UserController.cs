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
        IEncService enc;
        public UserController(IMongoService _db,IEncService _enc)
        {
            db = _db; enc = _enc;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await db.GetDocumentsAsync<UserModel>("Users",(new {}).ToBsonDocument() );
            return Ok(users.Take(25).ToArray());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var users = await db.GetDocumentsAsync<UserModel>("Users",(new {_id = id}).ToBsonDocument() );
            if(users.Count() < 1)
                return NotFound();
            return Ok(users.First());
        }

        [HttpPost("")]
        public async Task<IActionResult> PostUser(UserModel model)
        {
            model.PasswordHash = enc.EncryptPassword(model.PasswordHash);
            await db.InsertDocumentAsync<UserModel>("Users",model);
            return Ok(model.Cast<UserModel,UserViewModel>());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, UserModel model)
        {
            var users = await db.GetDocumentsAsync<UserModel>("Users",(new {_id = new ObjectId(id)}).ToBsonDocument() );
            if(users.Count() < 1)
                return NotFound();
            var result = await db.UpdateDocuments<UserModel>("Users",x => x.Id == id,model);
            if(result != 1)
                throw new Exception("Wrong query to database");
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            var users = await db.GetDocumentsAsync<UserModel>("Users",(new {_id = new ObjectId(id)}).ToBsonDocument() );
            if(users.ToList().Count() < 1)
                return NotFound();
            var result = await db.RemoveDocuments<UserModel>("Users",x => x.Id == id);
            if(result != 1)
                throw new Exception("Wrong query to database");
            return Ok();
        }
    }
}