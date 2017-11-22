using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NmeaParser.Business
{
    public class HDT : NMEA
    {
        public float heading = 0;
        //Symbol M (denotes that geoidal separation is in meters).
        public char headingType = 'T';

        #region Compose
        /// <summary>
        /// Compose HDT message string.
        /// </summary>
        /// <param name="heading"></param>
        /// <returns></returns>
        public String Compose(float heading)
        {
            this.heading = heading;

            return Compose();
        }

        public String Compose()
        {
            String hdt = "$GNHDT,";

            hdt += heading + "," + headingType;

            String cs = CalculateChecksum(hdt);

            return hdt + "*" + cs;
        }

        #endregion

        #region Parse

        public bool Parse(string message)
        {
            //$GPGGA,095257.00,5543.3503351,N,03739.0685196,E,1,16,0.76,148.9262,M,14.4309,M,,*51

            //string[] fields = GetFields(message.Trim(new char[] {'\r', '\n'}));
            string[] fields = GetFields(message.Trim());

            if (fields == null)
                return false;

            if (fields.Length != 4)
                return false;

            try
            {
                heading = ParseFloat(fields[1], 0);
                headingType = ParseChar(fields[2], 'T');
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
