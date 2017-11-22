using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NmeaParser.Business
{
    public class RMC : NMEA
    {
        public double latitude = 0;
        public double longitude = 0;
        public String status = String.Empty;
        public DateTime time;
       

        public double speed = 0;
        public double angle = 0;
        


        public RmcDto getRmcDtoPoit()
        {
            RmcDto rmc = new RmcDto();
            rmc.time = time;
           
            return rmc;
        }



        #region Parse

        public bool Parse(string message)
        {
            //$GPRMC,123519,A,4807.038,N,01131.000,E,022.4,084.4,230394,003.1,W * 6A

            Debug.WriteLine(message);

            //string[] fields = GetFields(message.Trim(new char[] {'\r', '\n'}));
            string[] fields = GetFields(message.Trim());

            if (fields == null)
                return false;

            if (fields.Length != 14)
                return false;

            try
            {
                int year = int.Parse(fields[9].Substring(4, 2));
                if (year<30)
                {
                    year += 2000;
                }

                time = new DateTime(
                    year,
                    int.Parse(fields[9].Substring(2, 2)),
                    int.Parse(fields[9].Substring(0, 2)),
                    int.Parse(fields[1].Substring(0, 2)),
                    int.Parse(fields[1].Substring(2, 2)),
                    int.Parse(fields[1].Substring(4, 2)));

                status = fields[2];

                latitude = ParseLatitude(fields[3], fields[4]);
                longitude = ParseLongitude(fields[5], fields[6]);

                speed = ParseDouble(fields[7], 0);
                angle = ParseDouble(fields[8], 0);

               
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
