using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SysOT.Models
{
    public class SeedModel
    {
        public IEnumerable<Device> Devices { get; set; }

        public IEnumerable<MeasurementType> MeasurementTypes { get; set; }

        public IEnumerable<MeasurementBucket> Measurements { get; set; }
    }
}