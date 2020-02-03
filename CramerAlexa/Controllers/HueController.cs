using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CramerAlexa.Hue.Classes;
using CramerAlexa.Services;
using CramerAlexa.Services.Interfaces;

namespace CramerAlexa.Hue.Controllers
{
    public class HueController : Controller
    {
        private readonly IHueService _hueService;
        private Global _globalSettings;

        public HueController(IHueService hueService, IOptions<Global> globalSettings)
        {
            _hueService = hueService;
            _globalSettings = globalSettings.Value;

        }

        [Route("api/setup.xml")]
        public string Setup()
        {
            hueTemplate = String.Format(hueTemplate, _globalSettings.IpAddress, _globalSettings.Port, "6c13659c-e3dc-4e08-9e0d-84c10f1ecb7e");
            return hueTemplate;
        }

        [Route("api/{userId}")]
        public async Task<HueApiResponse> Get(string userId)
        {
            Dictionary<string, DeviceResponse> deviceResponseList = new Dictionary<string, DeviceResponse>();

            List<Light> lights = await _hueService.GetLights();

            foreach (Light light in lights.Where(s => s.FriendlyName != null))
            {
                DeviceResponse newDR = DeviceResponse.createResponse(light.FriendlyName, light.FriendlyNameSlug, light.Mode == LightMode.on,
                    light.Percentage.HasValue ? light.Percentage.Value : 255);
                deviceResponseList.Add(light.FriendlyName, newDR);
            }

            return new HueApiResponse()
            {
                lights = deviceResponseList
            };
        }


        [HttpPut]
        [Route("api/{userId}/lights/{id}/{state}")]
        public async Task<IActionResult> SetState()
        {
            string id = Request.Path.Value.Split('/')[4];
            
            string content = new StreamReader(Request.Body).ReadToEndAsync().Result;
            DeviceState deviceState = Newtonsoft.Json.JsonConvert.DeserializeObject<DeviceState>(content);
            isOn = deviceState.on;
            if (deviceState.bri != 0)
            {
               await _hueService.DimLight(id, (deviceState.bri * 100 / 255), true);
            }
            else
            {
                await _hueService.SwitchLight(id, isOn ? LightMode.on : LightMode.off, true);
            }

            string responseString = "[{\"success\":{\"/lights/" + id.ToString() + "/state/on\":" + deviceState.on.ToString().ToLower() + "}}]";
            return Content(responseString, "application/json");
        }

        private bool isOn = false;

        [Route("api/{userId}/lights/{id}")]
        public async Task<DeviceResponse> GetLight(string id)
        {
            Light light = await _hueService.GetLight(id);

            DeviceResponse response = DeviceResponse.createResponse(light.FriendlyName, light.FriendlyNameSlug, 
                light.Mode == LightMode.on,
                light.Percentage.HasValue ? (light.Percentage.Value * 255 / 100) : 255);
            return response;
        }

        private string hueTemplate = "<?xml version=\"1.0\"?>\n" +
                  "<root xmlns=\"urn:schemas-upnp-org:device-1-0\">\n" +
                  "<specVersion>\n" +
                  "<major>1</major>\n" +
                  "<minor>0</minor>\n" +
                  "</specVersion>\n" +
                  "<URLBase>http://{0}:{1}/</URLBase>\n" + //hostname string
                  "<device>\n" +
                  "<deviceType>urn:schemas-upnp-org:device:Basic:1</deviceType>\n" +
                  "<friendlyName>Nano VeraHue Bridge</friendlyName>\n" +
                  "<manufacturer>Royal Philips Electronics</manufacturer>\n" +
                  "<manufacturerURL>http://www.nanoweb.com</manufacturerURL>\n" +
                  "<modelDescription>Hue to Vera Bridge for Amazon Echo</modelDescription>\n" +
                  "<modelName>Philips hue bridge 2012</modelName>\n" +
                  "<modelNumber>929000226503</modelNumber>\n" +
                  "<modelURL>http://www.nanoweb.com/HueVeraBridge</modelURL>\n" +
                  "<serialNumber>01189998819991197253</serialNumber>\n" +
                  "<UDN>uuid:{2}</UDN>\n" +
                  "<serviceList>\n" +
                  "<service>\n" +
                  "<serviceType>(null)</serviceType>\n" +
                  "<serviceId>(null)</serviceId>\n" +
                  "<controlURL>(null)</controlURL>\n" +
                  "<eventSubURL>(null)</eventSubURL>\n" +
                  "<SCPDURL>(null)</SCPDURL>\n" +
                  "</service>\n" +
                  "</serviceList>\n" +
                  "<presentationURL>index.html</presentationURL>\n" +
                  "<iconList>\n" +
                  "<icon>\n" +
                  "<mimetype>image/png</mimetype>\n" +
                  "<height>48</height>\n" +
                  "<width>48</width>\n" +
                  "<depth>24</depth>\n" +
                  "<url>hue_logo_0.png</url>\n" +
                  "</icon>\n" +
                  "<icon>\n" +
                  "<mimetype>image/png</mimetype>\n" +
                  "<height>120</height>\n" +
                  "<width>120</width>\n" +
                  "<depth>24</depth>\n" +
                  "<url>hue_logo_3.png</url>\n" +
                  "</icon>\n" +
                  "</iconList>\n" +
                  "</device>\n" +
                  "</root>\n";

       
    }
}
