using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;

namespace SSTI.IoT.Temperatura.Site.Controllers
{
    public class TemperaturasController : Controller
    {
        public IActionResult Index()
        {
            var model = new Models.Temperatura().Select();

            return View(model);
        }

        [HttpPost()]
        public IActionResult Index([FromBody]TemperaturaRequest request)
        {
            if (string.IsNullOrEmpty(request.Temperatura)) { return Json(new { Temperatura = 0 }); }
            var model = new Models.Temperatura().Insert(new Models.Temperatura.TemperaturaModel
            {
                Temperatura = float.Parse(request.Temperatura, System.Globalization.CultureInfo.GetCultureInfo("en-US"))
            });

            return Json(new { Temperatura = request.Temperatura });
        }

        [DataContract]
        public class TemperaturaRequest
        {
            [DataMember]
            public string Temperatura { get; set; }
        }
    }
}