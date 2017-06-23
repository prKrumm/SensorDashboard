using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.DTOs
{
    public class TemperaturMeasurement
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string uuid { get; set; }
        public string sensorid { get; set; }
        public int temperatur { get; set; }
        public int timestamp { get; set; }
    }
}
