using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SysOT.Models
{
    public class Device : BaseModel
    {
        [BsonElement("Name")]
        public string DeviceName { get; set; } = null!;
        
        public string IpAddress { get; set; }

        public string Type { get; set; }

        public bool Mobile { get; set; }
    }
}