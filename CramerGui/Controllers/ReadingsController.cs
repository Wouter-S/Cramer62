using CramerGui.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CramerGui
{
    public static class ExtensionMethods
    {
        public static string Replace(this string s, char[] separators, string newVal)
        {
            string[] temp;

            temp = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return String.Join(newVal, temp);
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ReadingsController : Controller
    {
        private IInfluxService _influxService;

        public ReadingsController(IInfluxService influxService)
        {
            _influxService = influxService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            Dictionary<string, string> readings = new Dictionary<string, string>();

            string tempJson = await _influxService.GetTemperatureReadings();
            string powerJson = await _influxService.GetPowerReadings();

            dynamic powerReponse = JsonConvert.DeserializeObject(powerJson);
            dynamic tempReponse = JsonConvert.DeserializeObject(tempJson);

            var d1 = ((IEnumerable)powerReponse.results[0].series).Cast<dynamic>()
                .ToDictionary(
                s => ((string)s.tags.type).Replace(new char[] { '-', '/' }, "_"),
                s => (string)s.values[0][1]);
            var d2 = ((IEnumerable)tempReponse.results[0].series).Cast<dynamic>()
               .ToDictionary(
               s => ((string)s.tags.type).Replace(new char[] { '-', '/' }, "_"),
               s => (string)s.values[0][1]);

            var merged = d1.Concat(d2)
                .ToLookup(x => x.Key, x => x.Value)
                .ToDictionary(x => x.Key, g => g.First());



            return Json(merged);
        }

        [HttpGet]
        [Route("time")]
        public ActionResult Time()
        {
            return Json(DateTime.Now);
        }


    }
}
