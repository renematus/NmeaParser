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
using System.Drawing;

namespace OGL_Library
{

	//WGS84 => TM2
	//WGS84toCartesian(Long, Lat) -> SevenParameter_BursaWolf(X, Y, Z) -> TMD67toEllipsoidal(X, Y, Z) -> TWD67EllipsoidaltoTM2(Long, lat);
	
	/*! \class GPSDatum
	 *  \brief The class provides some coordinate transformation methods(convert coordinates between longitude/latitude and easting northing. WGS84 to TM2).<BR>
	 *  WGS84 => TM2(Taiwan) <BR>
	 *  WGS84toCartesian(Long, Lat) -> SevenParameter_BursaWolf(X, Y, Z) -> TMD67toEllipsoidal(X, Y, Z) -> TWD67EllipsoidaltoTM2(Long, lat);
	 */
	public class GPSDatum
	{                          	
		//a, b are the lengths of the semi-major and semi-minor axes of the spheroid
		private const double WGS84a=6378137.0d; //the lengths of the semi-major axes of the spheroid
		private const double WGS84b=6356752.314d; //the lengths of the semi-minor axes of the spheroid	
		private const double TWD67a = 6378160.0d;
		private const double TWD67b = 6356774.719d;
		
		private const double DeltaX = 750.739d; //△X															   
		private const double DeltaY = 359.515d; //△Y	
		private const double DeltaZ = 180.510d; //△Z
		//Rx, Ry, Rz = The rotations around the three coordinate axes 
		//SC = The scale difference between the coordinate systems
		private const double Rx = 0.00003863d;
		private const double Ry = 0.00001721d;
		private const double Rz = 0.00000197d;
		private const double SC = 0.99998180d;

		private const double k0 = 0.9999d; //中央子午線尺度比
		private const double OffsetX = 250000.0d;	//東距(Ｘ軸)		
		private const double m_TWD67aSquared = (TWD67a * TWD67a);	
		private const double m_TWD67bSquared = (TWD67b * TWD67b);
		private const double m_WGS84aSquared = (WGS84a * WGS84a);
		private const double m_WGS84bSquared = (WGS84b * WGS84b);
		//e^2 = (a^2 - b^2) / a^2
		//(e')^2 = e^2 / (1 - e^2)
		private const double eccTWD67Squared = (m_TWD67aSquared - m_TWD67bSquared) / m_TWD67aSquared;		
		private const double eccPrimeTWD67Squared = (eccTWD67Squared)/(1-eccTWD67Squared);	
		private const double eccWGS84Squared = ((m_WGS84aSquared - m_WGS84bSquared)/ m_WGS84aSquared);
		private const double eccPrimeWGS84Squared = (eccWGS84Squared)/(1-eccWGS84Squared);
		

		public GPSDatum()
		{
			//Initialize
		}   

		//120.38330 => 120. 22' 59.88"
		/*!
		 * Parsing 120.38330 => 120. 22' 59.88"
		 * \param Long WGS84 longitude, Such as "120.38330".
		 * \return string[]<BR>[0]120<BR>[1]22<BR>[3]59.88
		 */
		public string[] ReturnAnalysisLongitude(string Long)
		{
			string[] NewArray = new string[3];
			string v1="",v2="";
			//extract 120
			v1 = Long.Split('.').GetValue(0).ToString();
			NewArray[0] = v1;
			//extract 0.38330
			v2 = "0."+Long.Split('.').GetValue(1).ToString();
			//extract and convert to ' unit
			v1 = Convert.ToSingle(float.Parse(v2)*60).ToString().Split('.').GetValue(0).ToString();
			NewArray[1] = v1;
			//extract and convert to " unit
			v2 = "0."+Convert.ToSingle(float.Parse(v2)*60).ToString().Split('.').GetValue(1).ToString();
			v2 = Convert.ToSingle(RoundOff(float.Parse(v2)*60,1)).ToString();

			NewArray[2] = v2;			
			return NewArray;
		}

		//22.769338 => 22. 46' 9.62"
		/*!
		 * Parsing 22.769338 => 22. 46' 9.62"
		 * \param Lat latitude Such as "22.769338".
		 * \return string[]<BR>[0]22<BR>[1]46<BR>[2]9.62
		 */
		public string[] ReturnAnalysisLatitude(string Lat)
		{
			string[] NewArray = new string[3];
			string v1="",v2="";
			//extract 120
			v1 = Lat.Split('.').GetValue(0).ToString();
			NewArray[0] = v1;
			//extract 0.38330
			v2 = "0."+Lat.Split('.').GetValue(1).ToString();
			//extract and convert to ' unit
			v1 = Convert.ToSingle(float.Parse(v2)*60).ToString().Split('.').GetValue(0).ToString();
			NewArray[1] = v1;
			//extract and convert to " unit
			v2 = "0."+Convert.ToSingle(float.Parse(v2)*60).ToString().Split('.').GetValue(1).ToString();
			v2 = Convert.ToSingle(RoundOff(float.Parse(v2)*60,1)).ToString();

			NewArray[2] = v2;			
			return NewArray;
		}		

		//WGS-84 Ellipsoidal => Cartesian coordinate transformation
		/*!
		 * WGS-84 Ellipsoidal => Cartesian coordinate transformation
		 * \param Long WGS84 longitude
		 * \param Lat WGS84 latitude
		 * \param h WGS84 Antenna altitude above/below mean sea level
		 * \return string[]<BR>[0] WGS84 Cartesian X<BR>[1] WGS84 Cartesian Y<BR>[2] WGS84 Cartesian Z<BR>
		 */
		public string[] WGS84toCartesian(double Long,double Lat, double h)
		{
			string[] NewArray = new string[3];
			double N = 0.0d; //卯酉圈之半徑(radius of curvature in prime vertical)
			double X = 0.0d,Y = 0.0d, Z = 0.0d; //

			//N is the radius of curvature in the prime vertical at the point
			//N = a^2 / (a^2 * cos^2(latitude) + b^2 * sin^2(latitude))*(1/2)
			//-----------------------------------------------------------------
			//sin^2(x)
			double sin2 = Math.Sin(this.ReturnRadian(Lat)) * Math.Sin(this.ReturnRadian(Lat));
			//cos^2(x)
			double cos2 = Math.Cos(this.ReturnRadian(Lat)) * Math.Cos(this.ReturnRadian(Lat));
			//q = (a^2 * cos^2(latitude) + b^2 * sin^2(latitude))
			double q = Math.Pow(WGS84a,2)*cos2+Math.Pow(WGS84b,2)*sin2;			
			//N = a^2 / q*1/2
			N = Math.Pow(WGS84a,2) / Math.Sqrt(q);
			//-----------------------------------------------------------------

			//X = (N + h) * cos(1) * cos(2)
			//Y = (N + h) * cos(1) * sin(2)
			//Z = ((b^2/a^2) * N + h) * sin(1)
			//1, 2 are the latitude, longitude of the point 
			//h Height of geoid (mean sea level) above WGS84 Ellipsoidal
			X = (N + h) * Math.Cos(this.ReturnRadian(Lat)) * Math.Cos(this.ReturnRadian(Long));
			Y = (N + h) * Math.Cos(this.ReturnRadian(Lat)) * Math.Sin(this.ReturnRadian(Long));
			Z = ((Math.Pow(WGS84b,2) / Math.Pow(WGS84a,2)) * N + h) * Math.Sin(this.ReturnRadian(Lat));

			NewArray[0] = X.ToString();
			NewArray[1] = Y.ToString();
			NewArray[2] = Z.ToString();

			return NewArray;
		}

		//WGS-84 Cartesian -> TMD67 Cartesian
		//Common Transformation Models
		//Seven Parameter Transformation (Bursa-Wolf Model) 
		/*!
		 * WGS-84 Cartesian -> TMD67 Cartesian<BR>
		 * Common Transformation Models<BR>
		 * Seven Parameter Transformation (Bursa-Wolf Model)<BR>
		 * \param X84 WGS84 Cartesian coordinate X
		 * \param Y84 WGS84 Cartesian coordinate Y
		 * \param Z84 WGS84 Cartesian coordinate Z
		 * \return string[]<BR> [0] TMD67 Cartesian X<BR>  [1] TMD67 Cartesian Y<BR>  [2] TMD67 Cartesian Z
		 */
		public string[] SevenParameter_BursaWolf(double X84,double Y84,double Z84)
		{
			string[] NewArray = new string[3];

			double X67=0.0d,Y67=0.0d,Z67=0.0d;

			//X67 = △X + SC * (X84 + Y84*Rz - Z84*Ry)
			//Y67 = △Y + SC * (-X84*Rz + Y84 + Z84*Rx)
			//Z67 = △Z + SC * (X84*Ry - Y84*Rx + Z84)
			//RX, RY, RZ = The rotations around the three coordinate axes 
			//SC = The scale difference between the coordinate systems
 
			/*X67 = DeltaX + SC * (X84 + Y84*Rz - Z84*Ry);
			Y67 = DeltaY + SC * (-X84*Rz + Y84 + Z84*Rx);
			Z67 = DeltaZ + SC * (X84*Ry - Y84*Rx + Z84);*/

			X67=X84+764.558;
			Y67=Y84+361.299;
			Z67=Z84+178.374;

			NewArray[0] = X67.ToString();
			NewArray[1] = Y67.ToString();
			NewArray[2] = Z67.ToString();

			return NewArray;
		}

		//TWD67 Cartesian -> TWD67(Taiwan Datum 67, TWD-67) Ellipsoidal
		/*!
		 * TWD67 Cartesian -> TWD67(Taiwan Datum 67, TWD-67) Ellipsoidal
		 * \param X67C TWD67 Cartesian coordinate X
		 * \param Y67C TWD67 Cartesian coordinate Y
		 * \param Z67C TWD67 Cartesian coordinate Z
		 * \return string[]<BR>[0] TWD67 longitude<BR>[1] TWD67 latitude<BR>[2] TWD67 Orthometric height<BR>
		 */
		public string[] TWD67toEllipsoidal(double X67C,double Y67C,double Z67C) 
		{
			string[] NewArray = new string[3];

			double P = Math.Sqrt(X67C * X67C + Y67C * Y67C);

			double Long67E =this.ReturnAngle(Math.Atan2(Y67C, X67C));						
			double O = Math.Atan2(Z67C*TWD67a,P*TWD67b);
			double Lat67E = this.ReturnAngle(Math.Atan2(Z67C+eccPrimeTWD67Squared*TWD67b*Math.Pow(Math.Sin(O),3),P-eccTWD67Squared*TWD67a*Math.Pow(Math.Cos(O),3)));	
			double N = Math.Sqrt(TWD67a / (1 - eccTWD67Squared*Math.Pow(Math.Sin(this.ReturnRadian(Lat67E)),2)));
			double h67E =  (P / Math.Cos(this.ReturnRadian(Lat67E))) - N;

			NewArray[0] = Long67E.ToString();
			NewArray[1] = Lat67E.ToString();
			NewArray[2] = h67E.ToString();

			return NewArray;
		} 		

		//UTM: Univeral Transverse Mercator, 橫麥卡托投影經差二度分帶.
		//麥卡扥圓柱投影為心射投影, 所得經緯線為正方位, 屬正向圖法.

		//TM2: 二度分帶座標.
		//台灣地區中央子午線為東經121度, 投影原點向西平移250,000公尺, 是為Ｙ軸,赤道為Ｘ軸.
		//澎湖、金門及馬祖等地區, 中央子午線定於東經119度, 投影原點向西平移250000公尺, 是為Ｙ軸, 赤道為Ｘ軸.

		// 座標系統	      中央經線	   東距(Ｘ軸) 	 縮尺系數    扁平率
        // 6度分帶(UTM)   123°(117°)   500,000公尺   0.9996	     1/297
        // 3度分帶(TM3)   121°(118°)   350,000公尺   1.0000	     1/298.25
		// 2度分帶(TM2)   121°(119°)   250,000公尺   0.9999	     1/298.25

		//TWD67(Taiwan Datum 67, TWD-67) Ellipsoidal -> TM2 (N,E)
		/*!
		 * TWD67(Taiwan Datum 67, TWD-67) Ellipsoidal -> TM2 (N,E)
		 * \param Long TWD67 longitude
		 * \param Lat TWD67 latitude
		 * \return string[]<BR>[0] TM2 easting<BR>[1] TM2 northing
		 */
		public string[] TWD67EllipsoidaltoTM2(double Long,double Lat)
		{
			string[] NewArray = new string[2];
			double UTMNorthing, UTMEasting;
			double N, T, C, A, M; 			
			double LatRad = this.ReturnRadian(Lat);
			double LongRad = this.ReturnRadian(Long);
			double LongOriginRad = this.ReturnRadian(121); //中央子午線
			
			//sin^2(x)
			double sin2 = Math.Sin(LatRad) * Math.Sin(LatRad); //**
			//cos^2(x)
			double cos2 = cos2 = Math.Cos(LatRad) * Math.Cos(LatRad); //**
			//q = (a^2 * cos^2(latitude) + b^2 * sin^2(latitude))
			double q = Math.Pow(TWD67a,2)*cos2+Math.Pow(TWD67b,2)*sin2;			
			//N = a^2 / q*1/2
			N = Math.Pow(TWD67a,2) / Math.Sqrt(q);
			//T = tan^2(x) = (1 / cos^2(x)) - 1
			//T = (1/cos2) - 1;
			T = Math.Tan(LatRad) * Math.Tan(LatRad); //**
			//C = (e')^2*cos^2(x)
			C = eccPrimeTWD67Squared*cos2;
			//A = cos(x) * long - 121;
			A = Math.Cos(LatRad)*(LongRad-LongOriginRad);

			M = TWD67a*((1-eccTWD67Squared/4-3*Math.Pow(eccTWD67Squared,2)/64-5*Math.Pow(eccTWD67Squared,3)/256)*LatRad 
				- (3*eccTWD67Squared/8+3*Math.Pow(eccTWD67Squared,2)/32+45*Math.Pow(eccTWD67Squared,3)/1024)*Math.Sin(2*LatRad)
				+ (15*Math.Pow(eccTWD67Squared,2)/256+45*Math.Pow(eccTWD67Squared,3)/1024)*Math.Sin(4*LatRad) 
				- (35*Math.Pow(eccTWD67Squared,3)/3072)*Math.Sin(6*LatRad));
			
			UTMEasting = OffsetX+(k0*N*(A+(1-T+C)*Math.Pow(A,3)/6+(5-18*T+T*T+72*C-58*eccPrimeTWD67Squared)*Math.Pow(A,5)/120));
			
			UTMNorthing = (k0*(M+N*Math.Tan(LatRad)*(Math.Pow(A,2)/2+(5-T+9*C+4*C*C)*Math.Pow(A,4)/24
				+ (61-58*T+T*T+600*C-330*eccPrimeTWD67Squared)*Math.Pow(A,6)/720)));

			NewArray[0] = UTMEasting.ToString();
			NewArray[1] = UTMNorthing.ToString();

			return NewArray;
		}     
		
		//WGS84 => TM2
		/*!
		 * Direct convert between WGS84 and TM2.
		 * \param Long WGS84 longitude
		 * \param Lat WGS84 latitude
		 * \return string[]<BR>[0] TM2 easting<BR>[1] TM2 northing
		 */
		public string[] WGS84toTM2(double Long,double Lat)
		{
			string[] NewArray = new string[2];

			GPSDatum g = new GPSDatum();			
			string[] s3 = g.WGS84toCartesian(Convert.ToDouble(Long),Convert.ToDouble(Lat),0);
			string[] s4 = g.SevenParameter_BursaWolf(Convert.ToDouble(s3[0]),Convert.ToDouble(s3[1]),Convert.ToDouble(s3[2]));
			string[] s5 = g.TWD67toEllipsoidal(Convert.ToDouble(s4[0]),Convert.ToDouble(s4[1]),Convert.ToDouble(s4[2]));
			string[] s6 = g.TWD67EllipsoidaltoTM2(Convert.ToDouble(s5[0]),Convert.ToDouble(s5[1]));
		
			NewArray[0] = s6[0];
			NewArray[1] = s6[1];

			return NewArray;
		}

		//WGS84 => TWD67
		/*!
		 * Direct convert between WGS84 and TWD67.
		 * \param Long WGS84 longitude
		 * \param Lat WGS84 latitude
		 * \return string[]<BR>[0] TWD67 longitude<BR>[1] TWD67 latitude
		 */
		public string[] WGS84toTWD67(double Long,double Lat,double h)
		{
			string[] NewArray = new string[3];

			GPSDatum g = new GPSDatum();			
			string[] s3 = g.WGS84toCartesian(Convert.ToDouble(Long),Convert.ToDouble(Lat),h);
			string[] s4 = g.SevenParameter_BursaWolf(Convert.ToDouble(s3[0]),Convert.ToDouble(s3[1]),Convert.ToDouble(s3[2]));
			string[] s5 = g.TWD67toEllipsoidal(Convert.ToDouble(s4[0]),Convert.ToDouble(s4[1]),Convert.ToDouble(s4[2]));
		
			NewArray[0] = s5[0];
			NewArray[1] = s5[1];
			NewArray[2] = s5[2];

			return NewArray;
		}

		//Angle convert Radian
		private double ReturnRadian(double angleValue)
		{
			const double DegreeToRadianRate = Math.PI / 180.0;
			return angleValue * DegreeToRadianRate;
		}

		//Radian convert Angle
		private double ReturnAngle(double radianValue)
		{
			const double RadianRateToDegree = 180.0 / Math.PI;
			return radianValue * RadianRateToDegree;
		}

		//四捨五入(RoundOff)
		private double RoundOff(double roundValue,int digits) 
		{ 
			double shift = Math.Pow(10,(double)digits); 
			return Math.Floor(roundValue * shift + 0.5) / shift; 
		} 
	}

	//地圖處理物件
	public class MAPXY
	{
		public struct PointF
		{
			public float X;
			public float Y;
		}
		
		private PointF pointLT;//地圖左上角經緯度座標
		private PointF pointRB;//地圖右下角經緯度座標
		private Point mapSIZE = new Point(0,0);//地圖寬高
		private Point showLT = new Point(0,0);//目前顯示之地圖左上角像素座標

		//MAP物件初始化
		public MAPXY()
		{
			pointLT.X = 0.0f; pointLT.Y = 0.0f;
			pointRB.X = 0.0f; pointRB.Y = 0.0f;
			mapSIZE.X = 0; mapSIZE.Y = 0;
			showLT.X = 0; showLT.Y = 0;
		}
		
		//設定地圖左上之實際座標
		public PointF MAP_POINT_LT
		{
			get{return pointLT;}
			set{pointLT.X=value.X; pointLT.Y=value.Y;}
		}

		//設定地圖右下之實際座標
		public PointF MAP_POINT_RB
		{
			get{return pointRB;}
			set{pointRB.X=value.X; pointRB.Y=value.Y;}
		}

		//設定地圖之像素寬高
		public Point MAP_SIZE
		{
			get{return mapSIZE;}
			set{mapSIZE.X = value.X; mapSIZE.Y = value.Y;}
		}
        
		//求兩數最小值
		private int MIN(int x, int y)
		{
			if(x>y)
				return y;
			else
				return x;
		}
		
		//求兩數最大值
		private int MAX(int x,int y)
		{
			if(x>y)
				return x;
			else
				return y;
		}

		//依照經緯度轉換為地圖像素座標
		public Point PointInMAP(PointF p)
		{
			PointF VarRate = new PointF();
			PointF VarLonLat = new PointF();
			Point PixelPoint = new Point(0,0);

			//計算
			//計算比率
			VarRate.X = pointRB.X - pointLT.X;
			VarRate.Y = pointRB.Y - pointLT.Y;
    
			//計算位置
			//公式(例) 0.24:662=0.12:x

			//取得參數
			VarLonLat.X = p.X;
			VarLonLat.Y = p.Y;

			//經度
			VarLonLat.X = VarLonLat.X - pointLT.X;
			VarLonLat.X = VarLonLat.X * MAP_SIZE.X;
			VarLonLat.X = VarLonLat.X / VarRate.X;
			
			//緯度
			VarLonLat.Y = VarLonLat.Y - pointLT.Y;
		    VarLonLat.Y = VarLonLat.Y * MAP_SIZE.Y;
			VarLonLat.Y = VarLonLat.Y / VarRate.Y;	

			//四捨五入			
			PixelPoint.X = Convert.ToInt32(VarLonLat.X);
			PixelPoint.Y = Convert.ToInt32(VarLonLat.Y);

			return PixelPoint;
		}

		//依照地圖像素轉換為經緯度座標
		public PointF PointInMAP(Point p)
		{
			PointF VarRate = new PointF();
			PointF VarLonLat = new PointF();
			PointF PixelPoint = new PointF();

			//計算
			//計算比率
			VarRate.X = MAP_SIZE.X;
			VarRate.Y = MAP_SIZE.Y;

			//經度
			VarLonLat.X = pointRB.X - pointLT.X;	
			VarLonLat.X = VarLonLat.X / VarRate.X;			
			VarLonLat.X = pointLT.X + (VarLonLat.X * p.X);
			
			//緯度
			VarLonLat.Y = pointLT.Y - pointRB.Y;	
			VarLonLat.Y = VarLonLat.Y / VarRate.Y;	
			VarLonLat.Y = pointRB.Y + (VarLonLat.Y * p.Y);

			//四捨五入			
			PixelPoint.X = (float)Convert.ToDouble(VarLonLat.X);
			PixelPoint.Y = (float)Convert.ToDouble(VarLonLat.Y);
			
			return PixelPoint;
		}	
	}

}
