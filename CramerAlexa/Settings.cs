using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CramerAlexa
{
    public class Global
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string LightApiBaseUrl { get; set; }
    }

    public class Mqtt
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
    }
}
