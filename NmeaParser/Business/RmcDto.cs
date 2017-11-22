using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NmeaParser.Business
{
    public class RmcDto
    {
        public double latitude = 0;
        public double longitude = 0;
        public String status = String.Empty;
        public DateTime time;


        public double speed = 0;
        public double angle = 0;
    }
}
