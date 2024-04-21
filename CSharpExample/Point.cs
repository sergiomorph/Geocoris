using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireBaseTest
{
    public class Point
    {
        //public Guid id { get; set; }
        [JsonProperty("x")]
        public double x { get; set; }

        [JsonProperty("y")]
        public double y { get; set; }

        [JsonProperty("z")]
        public double z { get; set; }

        public Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Point()
        {
            // Empty constructor needed for JSON serialization
        }
    }
}