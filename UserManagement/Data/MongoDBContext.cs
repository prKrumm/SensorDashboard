using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using UserManagement.DTOs;


namespace UserManagement.Data
{
    public class MongoDBContext
    {
        public string ConnectionString = "mongodb://userDSP:g8mYAonEarCBY55K@dbmongo/dbmongo";

        public string DatabaseName = "dbmongo";


        public static bool IsSSL { get; set; }

        public IMongoDatabase _database { get; }

        public MongoDBContext()
        {
            try
            {
                if (Environment.GetEnvironmentVariable("MONGODB_USER") != null)
                {
                    string user = Environment.GetEnvironmentVariable("MONGODB_USER");
                    string passwort = Environment.GetEnvironmentVariable("MONGODB_PASSWORD");

                    ConnectionString = "mongodb://" + user + ":" + passwort + "@dbmongo/dbmongo";
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

        public IMongoCollection<HelligkeitsMeasurement> Helligkeit(string Tenant)
        {
            return _database.GetCollection<HelligkeitsMeasurement>(Tenant);
        }


        public IMongoCollection<TemperaturMeasurement> Temperatur(string Tenant)
        {
            return _database.GetCollection<TemperaturMeasurement>(Tenant);
        }





    }
}
