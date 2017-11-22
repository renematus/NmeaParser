using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NmeaParser.Business
{
    public enum BasePositionType
    {
        NotAvailable,
        Autonomous,
        CodeDifferential,
        RTKFloat,
        RTKFixed
    }

    public class P_ATT : NMEA
    {
        public bool TimeIndicator;
        public DateTime time;
        public double Heading;
        public double Pitch;
        public BasePositionType PositionType;
        public double HeadingRMS;
        public double PitchRMS;
        public bool valid;

        //#region Compose
        ///// <summary>
        ///// Compose HDT message string.
        ///// </summary>
        ///// <param name="heading"></param>
        ///// <returns></returns>
        //public String Compose(float heading)
        //{
        //    this.heading = heading;

        //    return Compose();
        //}

        //public String Compose()
        //{
        //    String hdt = "$GNHDT,";

        //    hdt += heading + "," + headingType;

        //    String cs = CalculateChecksum(hdt);

        //    return hdt + "*" + cs;
        //}

        //#endregion

        #region Parse

        public bool Parse(string message)
        {
            //$PTPSR,ATT,V,093750.50,232.280,-2.972,N,0.001,0.001*05

            Debug.WriteLine(message);

            //string[] fields = GetFields(message.Trim(new char[] {'\r', '\n'}));
            string[] fields = GetFields(message.Trim());

            if (fields == null)
                return false;

            if (fields.Length != 10)
                return false;

            try
            {
                if (ParseChar(fields[2], 'N') == 'V')
                {
                    this.TimeIndicator = true;
                    this.valid = true;
                }
                else
                    this.TimeIndicator = false;

                this.time = new DateTime(
                    DateTime.Now.Year,
                    DateTime.Now.Month,
                    DateTime.Now.Day,
                    int.Parse(fields[3].Substring(0, 2)),
                    int.Parse(fields[3].Substring(2, 2)),
                    int.Parse(fields[3].Substring(4, 2)));

                if (fields[4] == "")
                    this.valid = false;

                this.Heading = ParseDouble(fields[4], 0);
                this.Pitch = ParseDouble(fields[5], 0);

                switch (ParseChar(fields[6], 'N'))
                {
                    case 'A':
                        this.PositionType = BasePositionType.Autonomous;
                        break;
                    case 'D':
                        this.PositionType = BasePositionType.CodeDifferential;
                        break;
                    case 'F':
                        this.PositionType = BasePositionType.RTKFloat;
                        break;
                    case 'R':
                        this.PositionType = BasePositionType.RTKFixed;
                        break;
                    default:
                        this.PositionType = BasePositionType.NotAvailable;
                        break;
                }

                this.HeadingRMS = ParseDouble(fields[7], 0);
                this.PitchRMS = ParseDouble(fields[8], 0);
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
