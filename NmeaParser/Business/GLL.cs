using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NmeaParser.Business
{
    public enum GLLStatus
    {
        Invalid = 'V',
        Autonomous = 'A',
        Differential = 'D',
        Precise = 'P',
        RTKFixed = 'R',
        RTKFloat = 'F'
    }

    public enum PositioningSystemMode
    {
        Autonomous = 'A',
        Differential = 'D',
        Estimated = 'E',
        Manual = 'M',
        Simulator = 'S',
        Invalid = 'N'
    }

    public class GLL : NMEA
    {
        public double latitude = 0;
        public double longitude = 0;
        public DateTime time;
        public bool overrideTime = false;
        public GLLStatus status = GLLStatus.Invalid;
        public PositioningSystemMode mode = PositioningSystemMode.Invalid;

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
            String gll = "$GPGLL,";
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



            gll += latd.ToString("00") + latm.ToString("00.00000") + "," + latDirection + ",";
            gll += lond.ToString("000") + lonm.ToString("00.00000") + "," + lonDirection + ",";
            gll += DateTime.Now.ToString("HHmmss.ss") + ",";
            gll += status + "," + mode;

            String cs = CalculateChecksum(gll);


            return gll + "*" + cs;
        }

        #endregion

        #region Parse

        public bool Parse(string message)
        {
           
            //$GNGLL,4105.0377442,N,02332.9868130,E,111117.00,A,A*7D

            //string[] fields = GetFields(message.Trim(new char[] {'\r', '\n'}));
            string[] fields = GetFields(message.Trim());

            if (fields == null)
                return false;

            if (fields.Length != 9)
                return false;

            try
            {
                latitude = ParseLatitude(fields[1], fields[2]);
                longitude = ParseLongitude(fields[3], fields[4]);

                time = new DateTime(
                    DateTime.Now.Year,
                    DateTime.Now.Month,
                    DateTime.Now.Day,
                    int.Parse(fields[5].Substring(0, 2)),
                    int.Parse(fields[5].Substring(2, 2)),
                    int.Parse(fields[5].Substring(4, 2)));

                status = (GLLStatus)ParseByte(fields[6], 0);
                mode = (PositioningSystemMode)ParseByte(fields[7], 0);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion

    }

}
