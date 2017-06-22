using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace TemperaturSensor
{
    class Program
    {

        MqttClient client;
        string serverTopic = "BaWue/77743/Haus1/temp-280591/Server";

        const string registerTopic = "Brightness/Register";
        string sensorTopic = "BaWue/77743/Haus1/temp-280591/Sensor";
        
        int periode;
        string SensorId;
        bool flag;

        public static void Main(string[] args)
        {
            Program program = new Program(args);

        }


        public Program(string[] args)
        {

            Console.Write("starting");
            //init vars
            periode = 5;
            serverTopic = "BaWue/77743/" + args[0] + "/temp-" + args[1] + "/Server";
            sensorTopic = "BaWue/77743/" + args[0] + "/temp-" + args[1] + "/Sensor";
            SensorId = "temp-" + args[1];
            flag = true;
            Console.Write(serverTopic +" "+serverTopic);

            client = new MqttClient("m13.cloudmqtt.com", 19401, false, null, null, MqttSslProtocols.None);
            client.ProtocolVersion = MqttProtocolVersion.Version_3_1;
            client.Connect("1235", "yvsytira", "ro0wLZInhaQo");
            client.Subscribe(new string[] { registerTopic, "/topic_2" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                  MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });




            client.MqttMsgPublished += client_MqttMsgPublished;
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            //Publish
            ushort msgId = client.Publish("/my_topic", // topic
                                          Encoding.UTF8.GetBytes("MyMessageBody"), // message body
                                          MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                                          false); // retained

            //Subscripe
            ushort msgId2 = client.Subscribe(new string[] { "/topic_1", "/topic_2" },
                    new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                  MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
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
            Debug.WriteLine("Received = " + Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic);
            var message = Encoding.UTF8.GetString(e.Message);

            CancellationTokenSource tokenSource = new CancellationTokenSource();

            switch (message)
            {
                //Connect
                case "CON:temp-280591":
                    flag = true;
                    //neue subscription
                    client.Subscribe(new string[] { registerTopic, serverTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                    MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
                    //sendet bestätigung
                    client.Publish("Brightness/Register", // topic
                                          Encoding.UTF8.GetBytes("CON:Success"), // message body
                                          MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                                          false); // retained
                                                  //Thread newThread = new Thread(new ThreadStart(sendSensorData));
                                                  // newThread.Start();
                                                  // newThread.IsBackground = true;

                    var taskWithToken = Task.Run(() =>
                   sendSensorData(), tokenSource.Token
                    );

                    break;
                //Disconnect
                case "DISC:temp-280591":
                    //unsubscripe
                    client.Unsubscribe(new string[] { serverTopic, "/topic_2" });
                    client.Publish("Brightness/Register", // topic
                                          Encoding.UTF8.GetBytes("DISC:Success"), // message body
                                          MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                                          false); // retained
                    flag = false;
                    tokenSource.Cancel();
                    break;


                default:
                    if (message.Contains("SYST:PER"))
                    {
                        //Periode ändern
                        // SYST:PER:1
                        int numberCount = message.Length - 9;
                        periode = Int32.Parse(message.Substring(9, numberCount));
                        tokenSource.Cancel();
                        taskWithToken = Task.Run(() =>
                       sendSensorData(), tokenSource.Token
                        );
                    }
                    break;
            }
        }

        void sendSensorData()
        {

            Random random = new Random();
            while (flag)
            {
                int sensorData = random.Next(80, 350);
                Thread.Sleep(periode * 1000);
                TemperaturDTO dto = new TemperaturDTO();
                dto.id = SensorId;
                dto.temperatur = sensorData;
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                dto.timestamp = unixTimestamp;

                string jsonDTO = JsonConvert.SerializeObject(dto);

                //{"id":"light-180591","helligkeit":12345,"timestamp":12345}
                //string msg= 
                //an MsgQ senden
                client.Publish(sensorTopic, // topic
                                             Encoding.UTF8.GetBytes(jsonDTO), // message body
                                             MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                                             false); // retained
            }

        }

    }
}