using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using EventHandler.DTOs;


namespace EventHandler.DB
{
    public class MongoDBContext
    {
        public string ConnectionString = "mongodb://patt:1234@ds151651.mlab.com:51651/appharbor_bl3d3vpj";
        
        public string DatabaseName = "appharbor_bl3d3vpj";
       
        
        public static bool IsSSL { get; set; }

        public IMongoDatabase _database { get; }

        public MongoDBContext()
        {
            try
            {
                if (Environment.GetEnvironmentVariable("MONGODB_USER") !=null)
                {
                    string user = Environment.GetEnvironmentVariable("MONGODB_USER");
                    string passwort = Environment.GetEnvironmentVariable("MONGODB_PASSWORD");

                    ConnectionString = "mongodb://" + user + ":" +passwort  + "@ds151651.mlab.com:51651/appharbor_bl3d3vpj";
                }

               var mongoClient = new MongoClient(ConnectionString);
               // var mongoClient = new MongoClient(testDB);

                _database = mongoClient.GetDatabase(DatabaseName);
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
