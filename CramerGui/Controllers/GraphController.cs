using CramerGui.Classes;
using CramerGui.Services;
using CramerGui.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace CramerGui.Controllers
{
    public class Reading
    {
        public Reading(int year, int month, decimal powerHigh, decimal powerLow)
        {
            Year = year;
            Month = month;
            PowerHigh = powerHigh;
            PowerLow = powerLow;
        }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal PowerHigh { get; set; }
        public decimal PowerLow { get; set; }
    }

    public class GraphController : Controller
    {
        private IRoomService _roomService;
        private IInfluxService _influxService;
        private GrafanaSettings _grafanaUrls;
        private readonly IHostingEnvironment _hostingEnvironment;

        public GraphController(IRoomService roomService, IInfluxService influxService, IOptions<GrafanaSettings> grafanaUrls, IHostingEnvironment hostingEnvironment)
        {
            _roomService = roomService;
            _influxService = influxService;
            _grafanaUrls = grafanaUrls.Value;
            _hostingEnvironment = hostingEnvironment;
        }

        [ResponseCache(Duration = 60)]
        [Route("/graph")]
        public ActionResult GetUsageData()
        {
            //Dictionary<int, List<Reading>> dict = new Dictionary<int, List<Reading>>();
            List<GraphRow> rows = new List<GraphRow>();

            HttpClient client = new HttpClient();
            for (int j = DateTime.Now.Year; j >= 2017; j--)
            {
                GraphRow graphRow = new GraphRow();
                List<Reading> readings = new List<Reading>();

                for (int i = 1; i <= 12; i++)
                {
                    string fromDate = $"{j}-{i.ToString("00")}-01 00:00:00";
                    string toDate = $"{(i == 12 ? j + 1 : j)}-{(i == 12 ? 1 : i + 1).ToString("00")}-01 00:00:00";

                    string json = _influxService.GetMonthlyTotals(fromDate, toDate);
                    dynamic reponse = JsonConvert.DeserializeObject(json);
                    var low = reponse?.results?[0].series?[0].values?[0][1];
                    var high = reponse?.results?[0].series?[1].values?[0][1];
                    if (low == null || high == null)
                    {
                        continue;
                    }
                    readings.Add(new Reading(j, i, decimal.Round((decimal)high), decimal.Round((decimal)low)));
                }
                graphRow.Year = j;
                graphRow.Readings = readings;
                rows.Add(graphRow);
            }
            var content = JsonConvert.SerializeObject(rows, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            return Content(content, "application/json");
        }

        [Route("/graphs/urls")]
        public ActionResult GetGraphUrls()
        {
            string baseUrl = _grafanaUrls.BaseUrl;
            _grafanaUrls.Last24Hrs = string.Format(_grafanaUrls.Last24Hrs, baseUrl);
            _grafanaUrls.Last7Days = string.Format(_grafanaUrls.Last7Days, baseUrl);
            _grafanaUrls.Last30Days = string.Format(_grafanaUrls.Last30Days, baseUrl);
            _grafanaUrls.Temperature = string.Format(_grafanaUrls.Temperature, baseUrl);
            _grafanaUrls.ScreenSaverPower = string.Format(_grafanaUrls.ScreenSaverPower, baseUrl);
            _grafanaUrls.ScreenSaverTemp = string.Format(_grafanaUrls.ScreenSaverTemp, baseUrl);

            return Json(_grafanaUrls);
        }

        [ResponseCache(Duration = 60)]
        [Route("/graph/image/{type}")]
        public ActionResult Image(string type, string theme)
        {
            string fileName = $"/images/graphs/{type}.jpg";
            string path = $"{_hostingEnvironment.WebRootPath}/{fileName}";

            //dont always want to download the file, randomize
            Random r = new Random();
            if (System.IO.File.Exists(path) && r.Next(0, 2) == 1)
            {
                return File($"~/{fileName}", "image/jpg");
            }

            using (WebClient client = new WebClient())
            {
                string property = (string)GetPropValue(_grafanaUrls, type);
                property = string.Format(property, _grafanaUrls.BaseUrl);

                client.DownloadFile(new Uri(property) + "&theme=" + theme, path);
                return File($"~/{fileName}", "image/jpg");
            }
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
