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


namespace NmeaParser
{
    public partial class Form1 : Form
    {
        private NMEA nmeaParser;
             
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            nmeaParser = new NMEA();
            nmeaParser.MessageReceived += NmeaParser_MessageReceived;

            String data = File.ReadAllText(@"c:\Vyvoj\SkolaFEI\MS\NMEA_Parser\NmeaParser\NmeaParser\Data\20090804-174057.nmea.txt");
            nmeaParser.AddData(data);
           
        }

        private void NmeaParser_MessageReceived(object sender, NMEAEventArgs e)
        {
           
        }
    }
}
