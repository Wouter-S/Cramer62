using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CramerAlexa.Hue.Classes
{
    public class Device
    {
        private long _id;

        public string id
        {
            get
            {
                return _id.ToString();
            }
            set
            {
                _id = long.Parse(value);
            }
        }
        public String name { get; set; }
        public String deviceType { get; set; }
        public String offUrl { get; set; }
        public String onUrl { get; set; }
        public String httpVerb { get; set; }
        public String contentType { get; set; }
        public String contentBody { get; set; }

    }
}
