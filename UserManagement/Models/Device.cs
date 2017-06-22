using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Models
{
    [Table("Devices")]
    public class Device
    {
        [Key]
        public int DeviceId { get; set; }
       

        public string DeviceName { get; set; }
        public DeviceType SensorType { get; set; }

        [ForeignKey("Id")]
       public ApplicationUser User { get; set; }



    }

    public enum DeviceType { LightSensor, TemperatureSensor }
}
