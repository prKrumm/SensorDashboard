using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.DTOs
{
    //{ time: Time, id: deviceName }
    public class MeasurementRequestDTO
    {
        public int time { get; set; }
        public string id { get; set; }
    }
}
