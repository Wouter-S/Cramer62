using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CramerGui
{
    public class InfluxSettings
    {
        public string BaseUrl { get; set; }
    }

    public class GrafanaSettings
    {
        public string BaseUrl { get; set; }
        public string Last7Days { get; set; }
        public string Last30Days { get; set; }
        public string Last24Hrs { get; set; }
        public string ScreenSaverPower { get; set; }
        public string ScreenSaverTemp { get; set; }
        public string Temperature { get; set; }
    }

    public class Mqtt
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
    }
}
