using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SysOT.Models
{
    public class Device : BaseModel
    {
        [BsonElement("Name")]
        [BsonRequired]
        public string DeviceName { get; set; } = null!;
        [BsonRequired]
        public string IpAddress { get; set; }

        public string Type { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("Desc")]
        public string Description { get; set; }

        public IEnumerable<string> Managers { get; set; }

        public bool Mobile { get; set; }
    }
}