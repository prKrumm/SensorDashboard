using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DTOs;

namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/Measurement")]
    public class MeasurementController : Controller
    {
        public MeasurementController()
        {

        }

        //GET: api/Measurement { time: Time, id: deviceName}
        [HttpGet]
        public JsonResult GetMeasurement([FromQuery]MeasurementRequestDTO item)
        {

            List<HelligkeitDTO> list = new List<HelligkeitDTO>();
            HelligkeitDTO dtoVar = new HelligkeitDTO();
            dtoVar.id = item.id;
            dtoVar.timestamp = item.time;

            list.Add(dtoVar);
            return Json(list);
        }


    }
}