using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {

        class Reading
        {
            public Reading(decimal year, decimal powerHigh, decimal powerLow)
            {
                Year = year;
                PowerHigh = powerHigh;
                PowerLow = powerLow;
            }
            public decimal Year { get; set; }
            public decimal PowerHigh { get; set; }
            public decimal PowerLow { get; set; }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            List<Reading> readings = new List<Reading>();

            HttpClient client = new HttpClient();
            //    string sensorReading = "1234";
            //    string bla = "power_usage,type=powerCurrent value=" + sensorReading;
            for(int j = 2016; j <= DateTime.Now.Year; j++) {
                Console.WriteLine(j);
                for (int i = 1; i <= 12; i++)
                {
                    string fromDate = $"{j}-{i.ToString("00")}-01 00:00:00";
                    string toDate = $"{(i == 12 ? j+1 : j)}-{(i == 12 ? 1 : i + 1).ToString("00")}-01 00:00:00";

                    var q = $"http://192.168.1.140:8086/query?q=SELECT (last(value) - first(value))  " +
                        $"FROM \"TheMachine\".\"autogen\".\"power_usage\" WHERE (time >= '{fromDate}' and time <= '{toDate}') " +
                        $"AND \"type\"='power-high' or \"type\"='power-low' " +
                        $"group by type " +
                        $"tz('Europe/Amsterdam')";
                    string json = Task.Run(() => client.GetAsync(q)).Result.Content.ReadAsStringAsync().Result;
                    dynamic test = JsonConvert.DeserializeObject(json);
                    var low = test?.results?[0].series?[0].values?[0][1];
                    var high = test?.results?[0].series?[1].values?[0][1];
                    if(low == null || high == null)
                    {
                        continue;
                    }
                    //readings.Add(
                    //    new Reading(
                    //        (decimal)high,
                    //        (decimal)low
                             
                            
                    //        )
                    //    );

                    Console.WriteLine(i + "\t" + decimal.Round((decimal)low) + "\t" + decimal.Round((decimal)high));
                }
            }
            Console.ReadLine();
        }
    }
}
