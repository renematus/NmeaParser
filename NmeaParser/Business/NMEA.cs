using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NmeaParser.Business
{
    public delegate void NMEAMessageEventHandler(object sender, NMEAEventArgs e);

    public class NMEAEventArgs : EventArgs
    {
        public string message;
        public string type;

        public NMEAEventArgs(string message, string type)
        {
            this.message = message;
            this.type = type;
        }
    }

    public class NMEA
    {
        string buffer;

        public event NMEAMessageEventHandler MessageReceived;

        public void AddData(string data)
        {
            int startIndex;
            int endIndex = 0;
            string message;

            buffer += data;

            while ((startIndex = buffer.IndexOf('$', endIndex)) >= 0)
            {
                endIndex = buffer.IndexOf("\n", startIndex);

                if (endIndex > 0)
                {
                    message = buffer.Substring(startIndex, endIndex - startIndex);

                    if (CompareChecksum(message))
                    {
                        Debug.WriteLine(message);
                        OnMessageReceived(new NMEAEventArgs(message, message.Substring(3, 3)));
                    }
                }
                else
                {
                    if ((startIndex = buffer.LastIndexOf('$')) > 0)
                        buffer = buffer.Remove(0, startIndex);
                    return;
                }
            }
        }

        #region Events

        protected virtual void OnMessageReceived(NMEAEventArgs e)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, e);
            }
        }

        #endregion

        #region CalculateChecksum
        /// <summary>
        /// Caulculates checksum of given NMEA message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Calculated 2 character checksum.</returns>
        public string CalculateChecksum(string message)
        {
            int chk = 0;
            int len = message.Length;

            if (message[len - 3] == '*')
            {
                len -= 3;
            }

            for (int i = 1; i < len; i++)
            {
                chk ^= message[i];
            }

            return String.Format("{0:X2}", chk);
        }

        #endregion

        #region CompareChecksum

        /// <summary>
        /// Compares checksum of given NMEA message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>True if checksum equals. False otherwise.</returns>
        public bool CompareChecksum(string message)
        {
            int i = message.LastIndexOf('*');

            if (i < 0)
                return false;

            if (message.Length - i < 3)
                return false;

            if (CalculateChecksum(message.Substring(0, i + 3)) == message.Substring(i + 1, 2))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region GetFields

        public string[] GetFields(string message)
        {
            //message.Trim({'\r', '\n'});
            //message = message.Trim(new char[] {'\r', '\n'});

            if (!CompareChecksum(message))
                return null;

            byte[] bMessage = Encoding.ASCII.GetBytes(message);

            if (bMessage[bMessage.Length - 3] == '*')
            {
                bMessage[bMessage.Length - 3] = Encoding.ASCII.GetBytes(",")[0];
            }

            char[] separator = { ',' };
            return Encoding.ASCII.GetString(bMessage).Split(separator);
        }

        #endregion

        #region Field parsers
        protected float ParseFloat(string value, float defaultValue)
        {
            try
            {
                return float.Parse(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }

        }

        protected double ParseDouble(string value, double defaultValue)
        {
            try
            {
                return double.Parse(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }

        }

        protected int ParseInt(string value, int defaultValue)
        {
            try
            {
                return int.Parse(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }

        }

        protected short ParseShort(string value, short defaultValue)
        {
            try
            {
                return short.Parse(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }

        }

        protected byte ParseByte(string value, byte defaultValue)
        {
            try
            {
                return byte.Parse(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }

        }

        protected char ParseChar(string value, char defaultValue)
        {
            try
            {
                return char.Parse(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }

        }

        protected double ParseLatitude(string value, string hemisphere)
        {
            double latitude;

            latitude = ParseDouble(value.Substring(0, 2), 0) + ParseDouble(value.Substring(2), 0) / 60;
            if (hemisphere.Contains("S"))
                latitude *= -1;

            return latitude;
        }

        protected double ParseLongitude(string value, string hemisphere)
        {
            double longitude;

            longitude = ParseDouble(value.Substring(0, 3), 0) + ParseDouble(value.Substring(3), 0) / 60;
            if (hemisphere.Contains("W"))
                longitude *= -1;

            return longitude;
        }

        #endregion


    }
}
