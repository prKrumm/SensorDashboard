using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandler.DTOs
{
    //{"id":"light-180591","temperatur":12345,"timestamp":12345}
    public class TemperaturDTO
    {
        public string id { get; set; }
        public int temperatur { get; set; }
        public int timestamp { get; set; }

    }
}
