using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.DTOs
{
    public class HelligkeitsMeasurement
    {
       
            public string uuid { get; set; }
            public string sensorid { get; set; }
            public int helligkeit { get; set; }
            public int timestamp { get; set; }
        
    }
}
