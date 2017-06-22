using EventHandler.DB;
using EventHandler.DTOs;

using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace EventHandler
{
    class Program
    {

        MqttClient client;
        string sensorTopic;
        MongoDBContext dbContext;

        static void Main(string[] args)
        {
            Program program = new Program(args);
            

            Console.WriteLine("Hello World!");
        }

        public Program(string[] args)
        {

            Console.Write("starting");
          
            sensorTopic = "BaWue/#";
          


            client = new MqttClient("m13.cloudmqtt.com", 19401, false, null, null, MqttSslProtocols.None);
            dbContext = new MongoDBContext();
            client.ProtocolVersion = MqttProtocolVersion.Version_3_1;
            client.Connect("1237", "yvsytira", "ro0wLZInhaQo");
            client.Subscribe(new string[] { sensorTopic, "/topic_3" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                  MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });




            client.MqttMsgPublished += client_MqttMsgPublished;
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            //Publish
            ushort msgId = client.Publish("/my_topic", // topic
                                          Encoding.UTF8.GetBytes("EventHandlerOnline"), // message body
                                          MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                                          false); // retained

           
        }

        void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Debug.WriteLine("MessageId = " + e.MessageId + " Published = " + e.IsPublished);
        }

        void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Debug.WriteLine("Subscribed for id = " + e.MessageId);
        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.Write("Received = " + Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic);
            var message = Encoding.UTF8.GetString(e.Message);
           
            if (message.Contains("temperatur"))
            {
                TemperaturDTO tempDto=JsonConvert.DeserializeObject<TemperaturDTO>(message);
                //Object dto = JsonConvert.DeserializeObject(message);
                TemperaturMeasurement tempMeasurement = new TemperaturMeasurement();
                tempMeasurement.sensorid = tempDto.id;
                tempMeasurement.temperatur = tempDto.temperatur;
                tempMeasurement.timestamp = tempDto.timestamp;
                tempMeasurement.uuid = Guid.NewGuid().ToString();
                dbContext.Temperatur(tempDto.id).InsertOne(tempMeasurement);
                Console.Write("Inserted:" + tempMeasurement);
            }
            if (message.Contains("helligkeit"))
            {
                HelligkeitDTO hellDTO = JsonConvert.DeserializeObject<HelligkeitDTO>(message);
                HelligkeitMeasurement hellMeasurement = new HelligkeitMeasurement();
                hellMeasurement.sensorid = hellDTO.id;
                hellMeasurement.timestamp = hellDTO.timestamp;
                hellMeasurement.helligkeit = hellDTO.helligkeit;
                hellMeasurement.uuid= Guid.NewGuid().ToString();
                dbContext.Helligkeit(hellDTO.id).InsertOne(hellMeasurement);
                Console.Write("Inserted:" + hellMeasurement);
            }


               



        }

       

    }



}
