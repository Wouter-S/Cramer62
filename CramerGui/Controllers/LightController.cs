
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CramerAlexa.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace CramerAlexa.Controllers
{
    public class LightController : Controller
    {
        private ILightService _lightService;

        public LightController(ILightService lightService)
        {
            _lightService = lightService;
        }

        [HttpGet]
        [Route("/api/lights")]
        public ActionResult GetLights(bool admin = false)
        {
            List<Light> lights = _lightService.GetLight();
            if (!admin)
            {
                lights.ForEach(l =>
                {
                    l.MqttAddress = "cramer62/lights/" + l.FriendlyNameSlug;
                });
            }
            return Json(lights);
        }

        [HttpGet]
        [Route("/api/lights/{lightFriendlyName}")]
        public ActionResult GetLights(string lightFriendlyName)
        {
            Light light = _lightService.GetLight(lightFriendlyName);
            light.MqttAddress = "cramer62/lights/" + light.FriendlyNameSlug;
            return Json(light);
        }

        [HttpPost]
        [Route("/api/lights")]
        public ActionResult SaveLights([FromBody] List<Light> lights)
        {
            _lightService.UpdateLights(lights);
            return Ok();
        }

        [HttpGet]
        [Route("/api/lights/{id}/{mode}")]
        public ActionResult SwitchLights(long id, string mode)
        {
            _lightService.SwitchLight(id, mode == "on" ? LightMode.on : LightMode.off, true);
            return Ok();
        }

        [HttpGet]
        [Route("/api/lights/logs")]
        public ActionResult GetLightLogs()
        {
            return Json(_lightService.GetLightLogs().OrderByDescending(l=>l.DateTime).ToList());
        }
    }
}
