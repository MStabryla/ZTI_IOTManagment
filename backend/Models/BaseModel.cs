using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Linq;

namespace SysOT.Models
{
    public class BaseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public T Cast<T,TBase>() where T : BaseModel {
            var type = typeof(T);
            var actTypePropertiesRaw = typeof(TBase).GetProperties();
            var actTypeProperties = typeof(TBase).GetProperties().Select(x => x.Name);
            T castedObject = Activator.CreateInstance<T>();
            foreach(var property in type.GetProperties()){
                if(actTypeProperties.Contains(property.Name))
                    property.SetValue(castedObject,actTypePropertiesRaw.First(x => x.Name == property.Name).GetValue(this));
            }
            return castedObject;
        }
    }
}