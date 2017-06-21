using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using EventHandler.DTOs;


namespace EventHandler.DB
{
    public class MongoDBContext
    {
        public const string ConnectionString = "mongodb://userMA4:XCNE32h4QG4fglxd@mongodb/sensorData";
        
        public const string DatabaseName = "sensorData";
       
        
        public static bool IsSSL { get; set; }

        public IMongoDatabase _database { get; }

        public MongoDBContext()
        {
            try
            {
               
                 
                //MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));
                //if (IsSSL)
                //{
                //    settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
                //}

               var mongoClient = new MongoClient(ConnectionSetting.CONNECTION_STRING);
               // var mongoClient = new MongoClient(testDB);

                _database = mongoClient.GetDatabase(ConnectionSetting.MONGODB_DATABASE);
                //_database = mongoClient.GetDatabase(testDatabaseName);

            }
            catch (Exception ex)
            {
                throw new Exception("Can not access to db server.", ex);
            }
        }

        public IMongoCollection<HelligkeitMeasurement> Helligkeit(string Tenant)
        {           
                return _database.GetCollection<HelligkeitMeasurement>(Tenant);          
        }


        public IMongoCollection<TemperaturMeasurement> Temperatur(string Tenant)
        {
            return _database.GetCollection<TemperaturMeasurement>(Tenant);
        }





    }
}
