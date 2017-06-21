using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandler.DTOs
{
    //{"id":"light-180591","helligkeit":12345,"timestamp":12345}
   public class HelligkeitDTO
    {
        public string id { get; set; }
        public int helligkeit { get; set; }
        public int timestamp { get; set; }
    }
}
