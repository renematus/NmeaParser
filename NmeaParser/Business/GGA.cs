using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NmeaParser.OGL_Library;
using static NmeaParser.OGL_Library.ConvertGPX;

namespace NmeaParser.Business
{
    public class GGA : NMEA
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


        public CWaypoint getPoit()
        {
            CWaypoint point = new CWaypoint();
            point.lat = latitude.ToString();
            point.lon = longitude.ToString();
            return point;
        }


        #region Compose
        /// <summary>
        /// Compose GGA message string.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public String Compose(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;

            return Compose();
        }

        public String Compose()
        {
            String gga = "$GPGGA,";
            DateTime d = DateTime.Now;

            String latDirection = "N";
            String lonDirection = "E";

            if (overrideTime)
                d = time;

            if (latitude < 0)
            {
                latitude = latitude *= -1;
                latDirection = "S";
            }

            if (longitude < 0)
            {
                longitude = longitude *= -1;
                lonDirection = "W";
            }


            int latd = (int)Math.Floor(latitude);
            int lond = (int)Math.Floor(longitude);

            double latm = (latitude - latd) * 60;
            double lonm = (longitude - lond) * 60;

            gga += DateTime.Now.ToString("HHmmss.ss") + ",";

            gga += latd.ToString("00") + latm.ToString("00.00000") + "," + latDirection + ",";
            gga += lond.ToString("000") + lonm.ToString("00.00000") + "," + lonDirection + ",";

            gga += quality + "," + numberOfSatellites + "," + hdop + "," + altitude + "," + altitudeUnits + ",";
            gga += geoidSeparation + "," + geoidSeparationUnit + "," + diffGPSAge + "," + refStatID;


            //gga = "$GPGGA,130426.40,5414.6090434,N,00047.5158980,W,1,17,0.68,21.9494,M,47.2000,M,,";
            //      "$GPGGA,152322.22,4900.00000  ,N,01800.00000  ,E,1, 0,0   ,0      ,M,      0,M,0,0*73"

            String cs = CalculateChecksum(gga);


            return gga + "*" + cs;
        }

        #endregion

        #region Parse

        public bool Parse(string message)
        {
            //$GPGGA,095257.00,5543.3503351,N,03739.0685196,E,1,16,0.76,148.9262,M,14.4309,M,,*51

            Debug.WriteLine(message);

            //string[] fields = GetFields(message.Trim(new char[] {'\r', '\n'}));
            string[] fields = GetFields(message.Trim());

            if (fields == null)
                return false;

            if (fields.Length != 16)
                return false;

            try
            {

                time = new DateTime(
                    DateTime.Now.Year,
                    DateTime.Now.Month,
                    DateTime.Now.Day,
                    int.Parse(fields[1].Substring(0, 2)),
                    int.Parse(fields[1].Substring(2, 2)),
                    int.Parse(fields[1].Substring(4, 2)));

                latitude = ParseLatitude(fields[2], fields[3]);
                longitude = ParseLongitude(fields[4], fields[5]);

                quality = ParseByte(fields[6], 0);
                numberOfSatellites = ParseByte(fields[7], 0);

                hdop = ParseFloat(fields[8], 99);
                altitude = ParseFloat(fields[9], 0);
                altitudeUnits = ParseChar(fields[10], 'M');
                geoidSeparation = ParseFloat(fields[11], 0);
                geoidSeparationUnit = ParseChar(fields[12], 'M');
                diffGPSAge = ParseFloat(fields[13], -1);
                refStatID = ParseShort(fields[14], 0);
            }
            catch (Exception)
            {
                Debug.WriteLine(message);
                return false;
            }


            return true;
        }

        #endregion

    }
}
