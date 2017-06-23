#include <SoftwareSerial.h>
#include <PubSubClient.h>
#include <WiFiEsp.h>
#include <WiFiEspClient.h>
#include <WiFiEspServer.h>
#include <WiFiEspUdp.h>
#include "TimerOne.h"

#define MAIN_TOPIC "Brightness/Register"
#define SUB_TOPIC "Brightness/light-180591/Server"
#define PUB_TOPIC "Brightness/light-180591/Sensor"

#define WIFI_AP "AndroidAP"
#define WIFI_PW "rirl8411" 


#define devID "light-180591"

const char* mqtt_broker = "broker.mqtt-dashboard.com";
char* chSUBTopic = MAIN_TOPIC;
char* chPUBTopic = MAIN_TOPIC;

int BrightnessPin = 1;
bool connectedToHost = false;

String oldFrame = String("");

WiFiEspClient espClient;
PubSubClient client(espClient);
SoftwareSerial serial(2,3);

int status = WL_IDLE_STATUS;
int lastSend;
long lastMsg = 0;
char msg[50];
int value = 0;

int defaultTime = 5;

unsigned long lastTime = 0;


void setup() {
  // initialize serial for debugging
  Serial.begin(9600);
  InitWiFi();
  lastSend = 0;
  client.setServer(mqtt_broker, 1883);
  client.setCallback(callback);

  lastTime = millis();
}
void loop() {
  if (!client.connected()) {
    reconnect();
  }
  client.loop();  
  unsigned long actualTIme = millis();
  
  if(actualTIme - lastTime > defaultTime*1000)
  {
    sendData();
    lastTime = actualTIme;
  }
}

void InitWiFi()
{
  // initialize serial for ESP module
  serial.begin(9600);
  // initialize ESP module
  WiFi.init(&serial);
  // check for the presence of the shield
  if (WiFi.status() == WL_NO_SHIELD) {
    Serial.println("WiFi shield not present");
    // don't continue
    while (true);
  }

  Serial.println("Connecting to AP ...");
  // attempt to connect to WiFi network
  while ( status != WL_CONNECTED) {
    Serial.print("Attempting to connect to WPA SSID: ");
    Serial.println(WIFI_AP);
    // Connect to WPA/WPA2 network
    status = WiFi.begin(WIFI_AP, WIFI_PW);
    delay(500);
  }
  Serial.println("Connected to AP");
}
void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    // Create a random client ID
    String clientId = "BenGehhAnPatKahh-";
    clientId += String(random(0xffff), HEX);
    // Attempt to connect
    if (client.connect(clientId.c_str())) {
      Serial.println("connected");
      // Once connected, publish an announcement...
      client.publish(chPUBTopic, "hello world");
      // ... and resubscribe
      client.subscribe(chSUBTopic);
      } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
}

void callback(char* topic, byte* payload, unsigned int length) {
  Serial.print("Message arrived [");
  Serial.print(topic);
  Serial.print("] ");
  String payloadstring = "";
  for (int i = 0; i < length; i++) {
    Serial.print((char)payload[i]);
    payloadstring += String((char)payload[i]);
    
  }
  
  process(payloadstring);
  Serial.println();
}
void process(String frameString)
{
    
   
    if(frameString.equals(oldFrame))
    {
      return;
    }
    else
    {
      oldFrame = frameString;    
      if(frameString.startsWith("CON"))
      {
        frameString.remove(0,4);
        if(frameString.equals(devID))
        {
          char* response = "CON:Success";
          client.publish(chSUBTopic, response);
          chSUBTopic = SUB_TOPIC;
          chPUBTopic = PUB_TOPIC;
          client.publish(chPUBTopic, "hello world");
          // ... and resubscribe
          client.subscribe(chSUBTopic);
          connectedToHost = true;
          Serial.print("Connected To A Host");
        }
        else
        {
          char* response = "CON:Failed";
          client.publish(chSUBTopic, response);
        }        
      }
      if(frameString.startsWith("DISC"))
      {
         frameString.remove(0,5);
        if(frameString.equals(devID))
        {
          char* response = "DISC:Success";
          client.publish(chSUBTopic, response);
          chSUBTopic = MAIN_TOPIC;
          chPUBTopic = MAIN_TOPIC;
                client.publish(chPUBTopic, "hello world");
      // ... and resubscribe
      client.subscribe(chSUBTopic);
          connectedToHost = false;
          Serial.print("Disconnected from Host"); 
        }
        else
        {
          char* response = "DISC:Failed";
          client.publish(chSUBTopic, response);
        }        
      }
      if(frameString.startsWith("SYST")){
        frameString.remove(0,5);
        if(frameString.startsWith("PER"))
        {
          Serial.print("Period Changed");
          frameString.remove(0,4);
          defaultTime = frameString.toInt();
        }
      }
    }
}
void sendData()
{
  
  
  if(connectedToHost){
    Serial.print("SendValue\n");
    int i32value = analogRead(BrightnessPin);
    String strValue = String(i32value);
      unsigned long currentMillis = millis();
      String Milliseconds = String(currentMillis);
      String response = "{\"id\":\"light-180591\",\"helligkeit\":";
      response += strValue;
      response += ",\"timestamp\":";
      response += Milliseconds;
      response += "}";
      client.publish(chPUBTopic, response.c_str());
  }
}



