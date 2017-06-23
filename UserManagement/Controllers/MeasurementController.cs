using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DTOs;
using UserManagement.Data;
using MongoDB.Driver;
using MongoDB.Bson;


namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/Measurement")]
    public class MeasurementController : Controller
    {
        MongoDBContext dbContext;
        public MeasurementController()
        {
            dbContext = new MongoDBContext();
        }

        //GET: api/Measurement { time: Time, id: deviceName}
        [HttpGet]
        public JsonResult GetMeasurement([FromQuery]MeasurementRequestDTO item)
        {
            string id = item.id;
            int time = item.time;
            int timeEnd = time + 86400;
            string json=null;
            if (id.Contains("light"))
            {
                var collection = dbContext.Helligkeit(item.id);
                var builder = Builders<HelligkeitsMeasurement>.Filter;
                var filter = builder.Gt("timestamp", time) & builder.Lt("timestamp", timeEnd);
                var result = collection.Find(filter).Project<HelligkeitsMeasurement>(Builders<HelligkeitsMeasurement>.Projection.Exclude(hamster => hamster._id)).ToList();
                json = result.ToJson();
            }
            if (id.Contains("temp"))
            {
                var collection = dbContext.Temperatur(item.id);
                var builder = Builders<TemperaturMeasurement>.Filter;
                var filter = builder.Gt("timestamp", time) & builder.Lt("timestamp", timeEnd);               
                var result = collection.Find(filter).Project<TemperaturMeasurement>(Builders<TemperaturMeasurement>.Projection.Exclude(hamster => hamster._id)).ToList();
                json = result.ToJson();
            }
           
            return Json(json);
        }


    }
}