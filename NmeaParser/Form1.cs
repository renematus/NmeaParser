using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NmeaParser.Business;
using System.Diagnostics;
using static NmeaParser.OGL_Library.ConvertGPX;

namespace NmeaParser
{
    public partial class Form1 : Form
    {
        private NMEA nmeaParser;
        private GGA gga;
        private HDT hdt;
        private P_ATT att;

        DateTime lastTime;

        List<CWaypoint> waypointList;


        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            waypointList = new List<CWaypoint>();
            hdt = new HDT();
            gga = new GGA();
            att = new P_ATT();
            nmeaParser = new NMEA();
            nmeaParser.MessageReceived += NmeaParser_MessageReceived;

            String data = File.ReadAllText(@"c:\Vyvoj\SkolaFEI\MS\NMEA_Parser\NmeaParser\NmeaParser\Data\2015-08-12.nmea.txt");
          //  nmeaParser.AddData(data);
           
        }

        private void NmeaParser_MessageReceived(object sender, NMEAEventArgs e)
        {
            switch (e.type)
            {
                case "GGA":
                    gga.Parse(e.message);
                    waypointList.Add(gga.getPoit());

                    if (waypointList.Count==100)
                    {
                        GPSConvertToGPX("test.gpx", waypointList.ToArray(), null);
                    }
                    break;

                case "HDT":
                    hdt.Parse(e.message);
                    break;

                case "PSR":
                    att.Parse(e.message);
                    LogData();

                    break;

                default:
                    Debug.WriteLine(e.message);
                    break;
            }
        }

        private void LogData()
        {
            if (gga.time != att.time)
                return;

            if (att.time == lastTime)
                return;

            string s = "";

            s += gga.time.ToString("dd/MM/yyyy HH:mm:ss.sss") + ",";
            s += gga.latitude.ToString("00.00000000") + ",";
            s += gga.longitude.ToString("000.00000000") + ",";
            s += gga.altitude.ToString("000.000") + ",";
            s += gga.quality.ToString("0") + ",";
            s += gga.numberOfSatellites.ToString("00") + ",";
            //s += att.time.ToString("dd/MM/yyyy HH:mm:ss.sss") + ",";
            s += att.Heading.ToString("000.000") + ",";
            s += att.HeadingRMS.ToString("000.000") + ",";
            s += att.Pitch.ToString("000.000") + ",";
            s += att.PitchRMS.ToString("000.000");

            //log.WriteLine(s);

            lastTime = att.time;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbSourceFile.Text = openFileDialog1.FileName;
            }
        }
    }
}
