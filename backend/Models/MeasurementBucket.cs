using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace SysOT.Models
{
    public class MeasurementBucket : BaseModel
    {
        [BsonRequired]
        public IEnumerable<object> Values { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public Device Device { get; set; }

        [BsonElement("Type")]
        public MeasurementType MeasurementType { get; set; }
        
    }
}