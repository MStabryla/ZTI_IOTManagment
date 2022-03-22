using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SysOT.Models
{
    public class Device
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string DeviceName { get; set; } = null!;
        
        public string IpAddress { get; set; }

        public string Type { get; set; }

        public bool Mobile { get; set; }
    }
}