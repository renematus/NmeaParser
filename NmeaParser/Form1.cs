﻿using System;
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
using System.Device.Location;

namespace NmeaParser
{
    public partial class Form1 : Form
    {
        private GPXLib gpx = new GPXLib();
        private NMEA nmeaParser;
        private GGA gga;
        private GLL gll;
        private RMC rmc; 

        DateTime lastTime;

        List<GgaDto> pointList;
        List<RmcDto> rmcList;


        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
           

           
           
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

                case "RMC":
                    rmc.Parse(e.message);
                    rmcList.Add(rmc.getRmcDtoPoit());
                    tbRMC.Invoke((Action)(() =>
                    {
                        tbRMC.Text = rmcList.Count.ToString();
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

        //private void LogData()
        //{
        //    if (gga.time != att.time)
        //        return;

        //    if (att.time == lastTime)
        //        return;

        //    string s = "";

        //    s += gga.time.ToString("dd/MM/yyyy HH:mm:ss.sss") + ",";
        //    s += gga.latitude.ToString("00.00000000") + ",";
        //    s += gga.longitude.ToString("000.00000000") + ",";
        //    s += gga.altitude.ToString("000.000") + ",";
        //    s += gga.quality.ToString("0") + ",";
        //    s += gga.numberOfSatellites.ToString("00") + ",";
        //    //s += att.time.ToString("dd/MM/yyyy HH:mm:ss.sss") + ",";
        //    s += att.Heading.ToString("000.000") + ",";
        //    s += att.HeadingRMS.ToString("000.000") + ",";
        //    s += att.Pitch.ToString("000.000") + ",";
        //    s += att.PitchRMS.ToString("000.000");

        //    //log.WriteLine(s);

        //    lastTime = att.time;
        //}

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
                    pointList = new List<GgaDto>();
                    rmcList = new List<RmcDto>();
                    gga = new GGA();
                    gll = new GLL();
                    rmc = new RMC();

                    nmeaParser = new NMEA();
                    nmeaParser.MessageReceived += NmeaParser_MessageReceived;

                    tbGpxFile.Text = String.Empty;
                    tbStatus.Text = String.Empty;
                    tbGGA.Text = "0";
                    tbRMC.Text = "0";
                    pointList = new List<GgaDto>();
                    rmcList = new List<RmcDto>();
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
            DateTime? date = null;
           
            int timeDiff=0;
            bool filtrByTime = false;
            if (!string.IsNullOrEmpty(tbFtime.Text) && int.TryParse(tbFtime.Text, out timeDiff))
            {
                filtrByTime = true;
            }

            int distance = 0;
            bool filtrByDistance = false;
            if (!string.IsNullOrEmpty(tbFdistance.Text) && int.TryParse(tbFdistance.Text, out distance))
            {
                filtrByTime = false;
                filtrByDistance = true;
            }


            List<Wpt> wayPoits = new List<Wpt>();
            int counter = 0;
            RmcDto rmc = null;
            foreach (var point in pointList)
            {
                if (rmcList.Count>counter)
                {
                    rmc = rmcList[counter++];
                    date = rmc.time;
                }

                Wpt wayPoint = new Wpt();
                wayPoint.Lat = (decimal)point.latitude;
                wayPoint.Lon = (decimal)point.longitude;

                if (point.altitude > 0)
                {
                    wayPoint.Ele = (decimal)point.altitude;
                    wayPoint.EleSpecified = true;
                }

                if (point.hdop > 0)
                {
                    wayPoint.Hdop = (decimal)point.hdop;
                    wayPoint.HdopSpecified = true;
                }

                if (point.geoidSeparation > 0)
                {
                    wayPoint.Geoidheight = (decimal)point.geoidSeparation;
                    wayPoint.GeoidheightSpecified = true;
                }

                
           


                if (date != null)
                {
                    wayPoint.Time = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, point.time.Hour, point.time.Minute, point.time.Second);
                }
                else
                {
                    wayPoint.Time = point.time;
                }
               
                wayPoint.TimeSpecified = true;


                if (filtrByTime)
                {
                    if (wayPoits.Count == 0 || Math.Abs((wayPoits.Last().Time - wayPoint.Time).TotalSeconds) > timeDiff)
                    {
                        wayPoits.Add(wayPoint);
                        gpx.AddTrackPoint("trackTimeFiltr", 0, wayPoint);

                        tbFiltrCount.Invoke((Action)(() =>
                        {
                            tbFiltrCount.Text = wayPoits.Count.ToString();
                        }));
                    }
                }
                else if (filtrByDistance)
                {
                    if (wayPoits.Count == 0)
                    {
                        wayPoits.Add(wayPoint);
                        gpx.AddTrackPoint("trackDistanceFiltr", 0, wayPoint);

                        tbFiltrCount.Invoke((Action)(() =>
                        {
                            tbFiltrCount.Text = wayPoits.Count.ToString();
                        }));
                    }
                    else
                    {
                        var aCoord = new GeoCoordinate((double)wayPoits.Last().Lat, (double)wayPoits.Last().Lon);
                        var bCoord = new GeoCoordinate((double)wayPoint.Lat, (double)wayPoint.Lon);

                        if (aCoord.GetDistanceTo(bCoord) > distance)
                        {
                            wayPoits.Add(wayPoint);
                            gpx.AddTrackPoint("trackDistanceFiltr", 0, wayPoint);

                            tbFiltrCount.Invoke((Action)(() =>
                            {
                                tbFiltrCount.Text = wayPoits.Count.ToString();
                            }));
                        }
                    }
                }
                else
                {
                    wayPoits.Add(wayPoint);
                    gpx.AddTrackPoint("trackNoFiltr", 0, wayPoint);
                }
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
