using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CramerAlexa.Services.Interfaces;

namespace CramerAlexa.Services
{
    public class InfluxService : IInfluxService
    {
        HttpClient client;
        private IOptions<InfluxSettings> _influxSettings;


        public InfluxService(IOptions<InfluxSettings> influxSettings)
        {
            client = new HttpClient();
            _influxSettings = influxSettings;
        }
        public async Task SendData(string sensorName, string sensorReading)
        {
            string message = $"power_usage,type={sensorName} value={sensorReading}";
            Console.WriteLine("Sending message to influx:" + message);

            var response = await client.PostAsync($"http://{_influxSettings.Value.BaseUrl}/write?db=TheMachine",
                new StringContent(message, Encoding.UTF8, "application/json"));
        }

        public async Task<string> GetTemperatureReadings()
        {
            var q = $"http://{_influxSettings.Value.BaseUrl}/query?q=SELECT last(value) " +
                       $"FROM \"TheMachine\".\"autogen\".\"temperature\" " +
                       $"group by type " +
                       " order by time desc " +
                       $"tz('Europe/Amsterdam')";
            HttpResponseMessage message = await client.GetAsync(q);
            return await message.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPowerReadings()
        {
            var q = $"http://{_influxSettings.Value.BaseUrl}/query?q=SELECT last(value) " +
                       $"FROM \"TheMachine\".\"autogen\".\"power_usage\" " +
                       $"group by type " +
                       " order by time desc " +
                       $"tz('Europe/Amsterdam')";
            HttpResponseMessage message = await client.GetAsync(q);
            return await message.Content.ReadAsStringAsync();
        }


        public string GetMonthlyTotals(string fromDate, string toDate)
        {
            var q = $"http://{_influxSettings.Value.BaseUrl}/query?q=SELECT (last(value) - first(value))  " +
                       $"FROM \"TheMachine\".\"autogen\".\"power_usage\" WHERE (time >= '{fromDate}' and time <= '{toDate}') " +
                       $"AND \"type\"='power-high' or \"type\"='power-low' " +
                       $"group by type " +
                       $"tz('Europe/Amsterdam')";
            return Task.Run(() => client.GetAsync(q)).Result.Content.ReadAsStringAsync().Result;
        }
    }
}
