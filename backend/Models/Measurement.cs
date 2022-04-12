using System;
using MongoDB.Bson.Serialization.Attributes;

namespace SysOT.Models
{
    public class Measurement<T> : BaseModel
    {
        [BsonRequired]
        public T Value { get; set; }
        
        [BsonRequired]
        public DateTime Time { get; set; }
    }
}