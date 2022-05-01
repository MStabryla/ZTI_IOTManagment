using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SysOT.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}