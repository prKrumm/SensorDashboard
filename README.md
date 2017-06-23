# SensorDashboard

Dieses Repository ist Teil eines Projektes des Moduls "Cloud Application Development".



# Arduino

  - Enthält den SourceCode für einen Arduino Uno
  - Vorraussetzungen: WLAN Modul, Lichtwiderstand
# TemperaturSensor

  - Simuliert einen TemperaturSensor mit folgenden Daten:
  - Paramter: Tenant: Haus1; Sensor: 380591
```sh
$ dotnet run Haus1 280591
```
# EventHandler
  - Service der Pub/Sub Pattern Messages mit einem MQTT MessageBroker austauscht
  - Vorraussetzungen: MQTT MessageBroker
  
# UserManagement
  - Web Frontend
  - Dashboard für Sensor Daten, die mittels Websockets angezeigt werden
  - verwendete Technologien: ASP.NET Core, Epoch.JS, Jquery, Bootstrap, Paho JavaScript Client
  
# Anleitung zum Ausprobieren:
1. Deployment
2. Im Dashboard einloggen mit folgenden Credentials:
- PW: Hallo123!
- Email: pa431kru@htwg-konstanz.de
4. TemperaturSensor starten mittels id aus datenbank
 ```sh
$ dotnet run Haus1 280591
```