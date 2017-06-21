using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandler.DB
{
    public class TemperaturMeasurement
    {
        public string uuid { get; set; }
        public string sensorid { get; set; }
        public int temperatur { get; set; }
        public int timestamp { get; set; }
    }
}
