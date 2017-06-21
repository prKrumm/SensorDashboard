using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using EventHandler.DTOs;


namespace EventHandler.DB
{
    public class MongoDBContext
    {
        public const string ConnectionString = "mongodb://userDSP:g8mYAonEarCBY55K@dbmongo/dbmongo";
        
        public const string DatabaseName = "dbmongo";
       
        
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
