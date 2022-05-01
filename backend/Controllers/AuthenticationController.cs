using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SysOT.Models;
using SysOT.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : Controller
    {
        IMongoService db;
        IEncService enc;
        public AuthenticationController(IMongoService _db,IEncService _enc)
        {   
            db = _db; enc = _enc;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            var userQuery = db.GetDocuments<User>("Users", (new {Email = model.Email}).ToBsonDocument());
            if(userQuery.Count() < 1)
                return Unauthorized();
            var user = userQuery.ElementAt(0);
            if(enc.EncryptPassword(model.Password) != user.PasswordHash)
                return Unauthorized();
            
            var token = enc.GenerateToken(user);
            return Ok(new {token});
        }

        [Authorize]
        [HttpGet("test")]
        public IActionResult TestAuthorize(){
            return Ok("Accepted");
        }
    }
}