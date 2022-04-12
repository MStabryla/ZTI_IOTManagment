using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SysOT.Models
{
    public class MeasurementType : BaseModel
    {
        [BsonElement("Name")]
        [BsonRequired]
        public string TypeName { get; set; } = null!;

        [BsonElement("Variable")]
        [BsonRequired]
        public string VariableType { get; set; } = null!;

        [BsonElement("Numeric")]
        public bool Numeric { get; set; }
    }
}