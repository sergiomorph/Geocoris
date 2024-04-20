using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireBaseTest
{
    public class Point1
    {
        //public Guid id { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        public Point1(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}