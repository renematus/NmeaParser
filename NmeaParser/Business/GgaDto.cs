using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NmeaParser.Business
{
    public class GgaDto
    {
        public double latitude = 0;
        public double longitude = 0;
        public DateTime time;
        public bool overrideTime = false;
        //GPS Quality Indicator
        public byte quality = 0;
        //Number of satellites used for position computation
        public byte numberOfSatellites = 0;
        //Horizontal dilution of precision (HDOP).
        public float hdop = 0;
        //Altitude above geoid
        public float altitude = 0;
        //Symbol M (denote that altitude is in meters).
        public char altitudeUnits = 'M';
        //Geoidal separation
        public float geoidSeparation = 0;
        //Symbol M (denotes that geoidal separation is in meters).
        public char geoidSeparationUnit = 'M';
        //Age of differential GPS data [seconds].
        public float diffGPSAge = 0;
        //Differential reference station ID (an integer between 0000 and 1023).
        public short refStatID;
    }
}
