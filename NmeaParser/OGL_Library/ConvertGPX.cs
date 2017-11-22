// Open GPS_LBS library.
// -------------------------------------------------
//  An open source library for GPS / LBS developers
// -------------------------------------------------
//
// Open GPS_LBS library is distributed under the GNU General Public License(gpl.txt). 
// Be sure to read it before using OGL-Library.
//
// Tsung-Te, Wu.(p3p3, Taiwan)
// <p3p3@mail2000.com.tw>
// <http://ogl-lib.sourceforge.net/index.html>

using System;
using System.Xml;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;

namespace NmeaParser.OGL_Library
{	
    /*! \class ConvertGPX
     *  \brief About GPX(the GPS eXchange Format) is a data format <BR> 
     *   for exchanging GPS data between programs, and for sharing GPS data <BR>
     *   with other users. Unlike other data files, which can only be understood <BR>
     *   by the programs that created them, GPX files actually contain<BR>
     *   a description of what's inside them, allowing anyone to create a program <BR>
     *   that can read the data within. This GPX class provide converts <BR>
     *   GPS data(waypoints, routes, and tracks) to be compatible with GPX format file..
     */
    public class ConvertGPX
    {       
       /*! \class CWaypoint
	 *  \brief GPS Waypoint record class..
         */
	public class CWaypoint
	{
		//Required Information
		private string vlat = "";			//<lat> Latitude of the waypoint
		private string vlon = "";			//<lon> Longitude of the waypoint	
	
		//Optional Position Information
		private string vele = "";			//<ele> Elevation of the waypoint
		private string vtime = "";			//<time> Creation date/time of the waypoint
		private string vmagvar = "";		//<magvar> Magnetic variation of the waypoint
		private string vgeoidheight = "";	//<geoidheight> Geoid height of the waypoint
		
		//Optional Description Information
		private string vname = "";			//<name> GPS waypoint name of the waypoint
		private string vcmt = "";			//<cmt> GPS comment of the waypoint
		private string vdesc = "";			//<desc> Descriptive description of the waypoint
		private string vsrc = "";			//<src> Source of the waypoint data
		private string vurl = "";			//<url> URL associated with the waypoint
		private string vurlname = "";		//<urlname> Text to display on the <url> hyperlink
		private string vsym = "";			//<sym> Waypoint symbol
		private string vtype = "";			//<type> Type (category) of waypoint

		//Optional Accuracy Information
		private string vfix = "";			//<fix> Type of GPS fix
		private string vsat = "";			//<sat> Number of satellites
		private string vhdop = "";			//<hdop> HDOP
		private string vvdop = "";			//<vdop> VDOP
		private string vpdop = "";			//<pdop> PDOP
		private string vageofdgpsdata = ""; //<ageofdgpsdata> Time since last DGPS fix
		private string vdgpsid = "";		//<dgpsid> DGPS station ID															 
		
		/** 
		 * <B>[Required]</B> Latitude of the waypoint.
                 */
		public string lat
		{
			get	{return vlat;}
			set	{vlat = value;}
		}	
		
		/** 
		 * <B>[Required]</B> Longitude of the waypoint.
                 */
		public string lon 
		{
			get	{return vlon;}
			set	{vlon = value;}
		}

		/** 
		 * <B>[Optional Position Information]</B> Elevation of the waypoint.
                 */
		public string ele 
		{
			get	{return vele;}
			set	{vele = value;}
		}	
		
		/** 
		 * <B>[Optional Position Information]</B> Creation date/time of the waypoint.
                 */
		public string time
		{
			get	{return vtime;}
			set	{vtime = value;}
		}	
		
		/** 
		 * <B>[Optional Position Information]</B> Magnetic variation of the waypoint.
                 */
		public string magvar
		{
			get	{return vmagvar;}
			set	{vmagvar = value;}
		}
		
		/** 
		 * <B>[Optional Position Information]</B> Geoid height of the waypoint.
                 */
		public string geoidheight
		{
			get	{return vgeoidheight;}
			set	{vgeoidheight = value;}
		}	
		
		/** 
		 * <B>[Optional Description Information]</B> GPS waypoint name of the waypoint.
                 */
		public string name
		{
			get	{return vname;}
			set	{vname = value;}
		}
		
		/** 
		 * <B>[Optional Description Information]</B> GPS comment of the waypoint.
                 */
		public string cmt
		{
			get	{return vcmt;}
			set	{vcmt = value;}
		}
		
		/** 
		 * <B>[Optional Description Information]</B> Descriptive description of the waypoint.
                 */
		public string desc
		{
			get	{return vdesc;}
			set	{vdesc = value;}
		}
		
		/** 
		 * <B>[Optional Description Information]</B> Source of the waypoint data.
                 */
		public string src
		{
			get	{return vsrc;}
			set	{vsrc = value;}
		}
		
		/** 
		 * <B>[Optional Description Information]</B> URL associated with the waypoint.
                 */
		public string url
		{
			get	{return vurl;}
			set	{vurl = value;}
		}
		
		/** 
		 * <B>[Optional Description Information]</B> Text to display on the <url> hyperlink.
                 */
		public string urlname
		{
			get	{return vurlname;}
			set	{vurlname = value;}
		}
		
		/** 
		 * <B>[Optional Description Information]</B> Waypoint symbol.
                 */
		public string sym
		{
			get	{return vsym;}
			set	{vsym = value;}
		}
		
		/** 
		 * <B>[Optional Description Information]</B> Type (category) of waypoint.
                 */
		public string type
		{
			get	{return vtype;}
			set	{vtype = value;}
		}
		
		/** 
		 * <B>[Optional Accuracy Information]</B> Type of GPS fix.
                 */
		public string fix
		{
			get	{return vfix;}
			set	{vfix = value;}
		}
		
		/** 
		 * <B>[Optional Accuracy Information]</B> Number of satellites.
                 */
		public string sat
		{
			get	{return vsat;}
			set	{vsat = value;}
		}
		
		/** 
		 * <B>[Optional Accuracy Information]</B> HDOP.
                 */
		public string hdop
		{
			get	{return vhdop;}
			set	{vhdop = value;}
		}
		
		/** 
		 * <B>[Optional Accuracy Information]</B> VDOP.
                 */
		public string vdop
		{
			get	{return vvdop;}
			set	{vvdop = value;}
		}
		
		/** 
		 * <B>[Optional Accuracy Information]</B> PDOP.
                 */
		public string pdop
		{
			get	{return vpdop;}
			set	{vpdop = value;}
		}
		
		/** 
		 * <B>[Optional Accuracy Information]</B> Time since last DGPS fix.
                 */
		public string ageofdgpsdata
		{
			get	{return vageofdgpsdata;}
			set	{vageofdgpsdata = value;}
		}
		
		/** 
		 * <B>[Optional Accuracy Information]</B> DGPS station ID.
                 */
		public string dgpsid
		{
			get	{return vdgpsid;}
			set	{vdgpsid = value;}
		}
	}
       
       /*! \class CRoute
	 *  \brief GPS Route record class..
         */
	public class CRoute
	{
		private string vname = "";    //<name> GPS route name 
		private string vcmt = "";     //<cmt> GPS route comment 
		private string vdesc = "";    //<desc> Description of the route 
		private string vsrc = "";     //<src> Source of the route data 
		private string vurl = "";     //<url> URL associated with the route 
		private string vurlname = ""; //<urlname> Text to display on the <url> hyperlink 
		private string vnumber = "";  //<number> GPS route number
		/** 
		 * <B>[Required]</B> List of routepoints(waypoints).
                 */
		public CWaypoint[] Routepoints; //List of routepoints(waypoint) 
		
		/** 
		 * <B>[Required]</B> GPS route name.
                 */
		public string name
		{
			get	{return vname;}
			set	{vname = value;}
		}
		
		/** 
		 * <B>[Optional]</B> GPS route comment.
                 */
		public string cmt
		{
			get	{return vcmt;}
			set	{vcmt = value;}
		}
		
		/** 
		 * <B>[Optional]</B> Description of the route.
                 */
		public string desc
		{
			get	{return vdesc;}
			set	{vdesc = value;}
		}
		
		/** 
		 * <B>[Optional]</B> Source of the route data.
                 */
		public string src
		{
			get	{return vsrc;}
			set	{vsrc = value;}
		}
		
		/** 
		 * <B>[Optional]</B> URL associated with the route.
                 */
		public string url
		{
			get	{return vurl;}
			set	{vurl = value;}
		}
		
		/** 
		 * <B>[Optional]</B> Text to display on the <url> hyperlink.
                 */
		public string urlname
		{
			get	{return vurlname;}
			set	{vurlname = value;}
		}
		
		/** 
		 * <B>[Optional]</B> GPS route number.
                 */
		public string number
		{
			get	{return vnumber;}
			set	{vnumber = value;}
		}
																																																						
	}
       
       
       /*! 
	* GPS Data convert to GPX format file.<BR>
	* See more : <A HREF="http://www.topografix.com/gpx.asp">http://www.topografix.com/gpx.asp</A>
	* \param SaveFileName Save file name.
	* \param vWaypoints CWaypoint Array, or <B>null</B>.
	* \param vRoutes CRoute Array, or <B>null</B>.
	* \return Save successful (true / false)
	*/
       public static bool GPSConvertToGPX(string SaveFileName, CWaypoint[] vWaypoints, CRoute[] vRoutes)
	   {
          XmlTextWriter myXmlTextWriter = null;
          myXmlTextWriter = new XmlTextWriter(SaveFileName, null);

          try
          {
			myXmlTextWriter.Formatting = Formatting.Indented;

			myXmlTextWriter.WriteStartDocument();
	    
            myXmlTextWriter.WriteStartElement("gpx");

			myXmlTextWriter.WriteAttributeString("version","1.0");
			myXmlTextWriter.WriteAttributeString("creator","p3p3-NSC92-2815-C-366-004-E");
			myXmlTextWriter.WriteAttributeString("xmlns:xsi","http://www.w3.org/2001/XMLSchema-instance");
			myXmlTextWriter.WriteAttributeString("xmlns","http://www.topografix.com/GPX/1/1");
			myXmlTextWriter.WriteAttributeString("xsi:schemaLocation","http://www.topografix.com/GPX/1/1/gpx.xsd");
 
			myXmlTextWriter.WriteElementString("time", "2004-02-06T08:57:21Z");

		    myXmlTextWriter.WriteStartElement("bounds");
			myXmlTextWriter.WriteAttributeString("minlat","43.299260");
			myXmlTextWriter.WriteAttributeString("minlon","-116.523480");
			myXmlTextWriter.WriteAttributeString("maxlat","43.299260");
			myXmlTextWriter.WriteAttributeString("maxlon","-116.523480");
			myXmlTextWriter.WriteEndElement();

			  
			if(vWaypoints!=null)
			{
				for(int countWP=0;countWP<vWaypoints.Length;countWP++)
				{
					myXmlTextWriter.WriteStartElement("wpt");

					CWaypoint WP = vWaypoints[countWP];	

					//Required Informations
					myXmlTextWriter.WriteAttributeString("lat", WP.lat);
					myXmlTextWriter.WriteAttributeString("lon", WP.lon);

					//Optional Information
					Type t = WP.GetType();
					PropertyInfo[] PIS = t.GetProperties();
					
					foreach(PropertyInfo pi in PIS)
					{
						if((pi.Name.Equals("lat")!=true)&&(pi.Name.Equals("lon")!=true)&&(pi.GetValue(WP,null).ToString().Length!=0))
						{							
							myXmlTextWriter.WriteElementString(pi.Name,pi.GetValue(WP,null).ToString());
						}
					}					
   				    
					myXmlTextWriter.WriteEndElement();
					
					t = null;
					PIS = null;
					WP = null;
				} 
			}

			if(vRoutes!=null)
			{
				for(int countRT=0;countRT<vRoutes.Length;countRT++)
				{			
					myXmlTextWriter.WriteStartElement("rte");

					//Optional Information
					CRoute RT = vRoutes[countRT];
					Type t = RT.GetType();
					PropertyInfo[] PIS = t.GetProperties();
					
					foreach(PropertyInfo pi in PIS)
					{
						if(pi.GetValue(RT,null).ToString().Length!=0)
						{							
							myXmlTextWriter.WriteElementString(pi.Name,pi.GetValue(RT,null).ToString());
						}
					}
					
					//<rtept>----------------------------------------------					
					if(RT.Routepoints!=null)
					{
						for(int countRoutepoints=0;countRoutepoints<RT.Routepoints.Length;countRoutepoints++)
						{
							myXmlTextWriter.WriteStartElement("rtept");
						
							CWaypoint WP = RT.Routepoints[countRoutepoints];	

							//Required Informations
							myXmlTextWriter.WriteAttributeString("lat", WP.lat);
							myXmlTextWriter.WriteAttributeString("lon", WP.lon);

							//Optional Information
							Type tWP = WP.GetType();
							PropertyInfo[] PIS_WP = tWP.GetProperties();
					
							foreach(PropertyInfo pi in PIS_WP)
							{
								if((pi.Name.Equals("lat")!=true)&&(pi.Name.Equals("lon")!=true)&&(pi.GetValue(WP,null).ToString().Length!=0))
								{							
									myXmlTextWriter.WriteElementString(pi.Name,pi.GetValue(WP,null).ToString());
								}
							}

							myXmlTextWriter.WriteEndElement();

							tWP = null;
							PIS_WP = null;
							WP = null;
						}
					}
					//</rtept>---------------------------------------------
   				    
					myXmlTextWriter.WriteEndElement();
					
					t = null;
					PIS = null;
					RT = null;

				}				
			}
                
            myXmlTextWriter.WriteEndElement();

            myXmlTextWriter.WriteEndDocument();

          }
          catch(Exception e)
          {
             Console.WriteLine("Exception: {0}", e.ToString());
          }
          finally
          {
             if (myXmlTextWriter != null)
             {
                myXmlTextWriter.Close();
             }		     
          }

			return true;
       }
    }
}
