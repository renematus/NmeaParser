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
using MKCoolsoft.GPXLib;

namespace NmeaParser
{
    public partial class Form1 : Form
    {
        private GPXLib gpx = new GPXLib();
        private NMEA nmeaParser;
        private GGA gga;
        private HDT hdt;
        private P_ATT att;

        DateTime lastTime;

        List<GgaDto> pointList;


        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            pointList = new List<GgaDto>();
            hdt = new HDT();
            gga = new GGA();
            att = new P_ATT();
            nmeaParser = new NMEA();
            nmeaParser.MessageReceived += NmeaParser_MessageReceived;

           
           
        }

        private void NmeaParser_MessageReceived(object sender, NMEAEventArgs e)
        {
            switch (e.type)
            {
                case "GGA":
                    gga.Parse(e.message);
                    pointList.Add(gga.getGgaDtoPoit());

                    tbGGA.Invoke((Action) (() =>
                        {
                            tbGGA.Text = pointList.Count.ToString();
                    }));
                    break;
                case "FINISH":
                    converseToGPX();
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
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            openFileDialog1.InitialDirectory = path;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbSourceFile.Text = openFileDialog1.FileName;
            }
        }

        private void btnParseFile_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbSourceFile.Text))
            {
                if (File.Exists(tbSourceFile.Text))
                {
                    tbGpxFile.Text = String.Empty;
                    tbStatus.Text = String.Empty;
                    tbGGA.Text = "0";
                    pointList = new List<GgaDto>();
                    String data = File.ReadAllText(tbSourceFile.Text);
                    Task parserTask = new Task( () => nmeaParser.AddData(data));
                    parserTask.Start();
                }
                else
                {
                    MessageBox.Show("Zadany soubor neexistuje", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void converseToGPX()
        {
            List<Wpt> wayPoits = new List<Wpt>();
            foreach (var point in pointList)
            {
                Wpt wayPoint = new Wpt();
                wayPoint.Lat = (decimal)point.latitude;
                wayPoint.Lon = (decimal)point.longitude;
                wayPoint.Ele = (decimal)point.altitude;
                wayPoits.Add(wayPoint);
                gpx.AddTrackPoint("trackXXX", 0, wayPoint);
            }

            gpx.SaveToFile("Data\\Result.gpx");

            
                tbGpxFile.Invoke((Action)(() =>
                {
                    string fileName = Path.Combine(Directory.GetCurrentDirectory(), "Data\\Result.gpx");
                    tbGpxFile.Text = fileName;
                    tbStatus.Text = "Konverze nmea to GPX OK";
                }));
           
            //else
            //{
            //    tbStatus.Invoke((Action)(() =>
            //    {
            //        tbStatus.Text = "Konverze nmea to GPX skoncila s chybou";
            //    }));
            //}
        }
    }
}
