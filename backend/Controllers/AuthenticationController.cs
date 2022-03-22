using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : Controller
    {
        public AuthenticationController()
        {
            
        }

        [HttpGet("Test")]
        public IActionResult Test(){
            return Ok(new { Text = "Test" });
        }
    }
}