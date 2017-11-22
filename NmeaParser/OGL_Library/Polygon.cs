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
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;

namespace OGL_Library
{	
	/*! \class Layer
	 *  \brief The class provide Layer manage. Add some Polygon or Remove it, <BR>
	 *   and provide PointInPolygon method to determine XY point(Screen pixel) is in what Polygon of layer?
         */
	public class Layer : CollectionBase
	{		
		/*!
		 * Add Polygon.
		 * \param PolygonToAdd Polygon type.
		 */
		public void Add(Polygon PolygonToAdd)
		{
			List.Add(PolygonToAdd);
		}
		
		/*!
		 * Remove Polygon.
		 * \param PolygonToRemove Polygon type.
		 */
		public void Remove(Polygon PolygonToRemove)
		{
			List.Remove(PolygonToRemove);
		}
		
		/*!
		 * Remove Polygon by index.
		 * \param Index int type.
		 */
		public void Remove(int Index)
		{
			List.RemoveAt(Index);
		}
		
		/*!
		 * Get some Polygon by index.
		 * \param Index int type.
		 * \return Polygon type
		 */
		public Polygon this[int Index]
		{
			get
			{
				return (Polygon)List[Index];
			}
			set
			{
				List[Index] = value;
			}
		}	
	
		//取得座標所位於的Polygon
		/*!
		 * determine XY point(Screen pixel) is in what Polygon of layer.
		 * \param XY Point type.
		 * \return Polygon index that int type
		 */
		public int PointInPolygon(Point xy)
		{
			for(int iCount=0;iCount<=List.Count-1;iCount++)
			{
				Polygon p = (Polygon)List[iCount];
				if(p.CourseInPolygon(xy))
				{
					return iCount;	
					break;					
				}
			}

			return -1;
		}
	}
	
	/*! \class Polygon
	 *  \brief The class provide ploygon manage. An example would be someone using their GPS-PDA <BR>
	 *   to zoo guiding tour and it would to determine the user's location and provide the animal <BR>
	 *   info from before mine eyes. The "Polygon class" is based on an algorithm which detections <BR>
	 *   Point(GPS geodetic position) in the Polygon(GPS-Area)..
         */
	public class Polygon
	{		
		
		private ArrayList myPts;

		public Polygon()
		{
			myPts = new ArrayList();
		}
		
		/** 
		 * Draw all points of ploygon in Bitmap than can display on the screen.
		 * \param DrawBMP Bitmap
		 * \param L left position of device screen
		 * \param T left position of device screen
                 */
		public void DrawPloygonPoint(Bitmap DrawBMP,int L,int T)
		{
			//將所有紀錄Point繪製出來
			foreach(Point p in myPts)
				Graphics.FromImage(DrawBMP).FillRectangle(new SolidBrush(Color.DarkBlue),p.X-2-L,p.Y-2-T,5,5);
		}
		
		/** 
		 * Draw all lines of ploygon in Bitmap than can display on the screen.
		 * \param DrawBMP Bitmap
		 * \param L left position of device screen
		 * \param T left position of device screen
                 */
		public void DrawPloygon(Bitmap DrawBMP,int L,int T)
		{
			//將所有紀錄線條繪製出來
			for(int iCount=0;iCount<myPts.Count;iCount++)
			{																			
				Point pStart = (Point)myPts[iCount];
				if(iCount+1<myPts.Count)
				{
					Point pEnd = (Point)myPts[iCount+1];
					Graphics.FromImage(DrawBMP).DrawLine(new Pen(Color.LightSkyBlue),pStart.X-L,pStart.Y-T,pEnd.X-L,pEnd.Y-T);
				}				
			}
			
			if(myPts.Count>2)
			{
				Point pStart2 = (Point)myPts[0];
				Point pEnd2 = (Point)myPts[myPts.Count-1];
				Graphics.FromImage(DrawBMP).DrawLine(new Pen(Color.LightSkyBlue),pStart2.X-L,pStart2.Y-T,pEnd2.X-L,pEnd2.Y-T);
			}
		}
		
		//取得封閉多邊形之各點
		/** 
		 * Get all point of the ploygon.
		 * \return Point[](Point Array)
                 */
		public Point[] PolygonAllPoint()
		{
			Point[] AllPoint = new Point[myPts.Count];
			for(int iCount=0;iCount<myPts.Count;iCount++)
			{
				AllPoint[iCount] = (Point)myPts[iCount];
			}
			
			return AllPoint;
		}

		//取得封閉多邊形之各點
		/** 
		 * Get all point of the ploygon.
		 * \return String(Format => x1,y1;x2,y2;x3,y3)
                 */
		public string PolygonAllPoint_str()
		{
			 string AllPoint = "";
			 for(int iCount=0;iCount<myPts.Count;iCount++)
			 {
				 Point p = (Point)myPts[iCount];
				 AllPoint += p.X+","+p.Y+";";
			 }
			 AllPoint = AllPoint.Substring(0,AllPoint.Length-1);
			 return AllPoint;
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

		//判斷點是否在多邊形內
		private Boolean InsidePolygon(ref Point[] polygon,int N,Point p)
		{		
			int counter = 0;
			int i;
			double xinters;
			Point p1,p2;			

			p1 = polygon[0];
			for (i=1;i<=N;i++) 
			{
				p2 = polygon[i % N];
				if (p.Y > MIN(p1.Y,p2.Y)) 
				{
					if (p.Y <= MAX(p1.Y,p2.Y)) 
					{
						if (p.X <= MAX(p1.X,p2.X)) 
						{
							if (p1.Y != p2.Y) 
							{
								xinters = (p.Y-p1.Y)*(p2.X-p1.X)/(p2.Y-p1.Y)+p1.X;
								if (p1.X == p2.X || p.X <= xinters)
									counter++;
							}
						}
					}
				}
				p1 = p2;
			}

			if (counter % 2 == 0)
				return false;
			else
				return true;
		}		
		
		//判斷點是否在多邊形內
		/** 
		 * Detections Point(GPS geodetic position) in the Polygon(GPS-Area)..
		 * \param CourseXY XY Point type
		 * \return Boolean type
                 */
		public Boolean CourseInPolygon(Point CourseXY)
		{
			if(myPts.Count<3) return false;
			Boolean InPolygon = false;
			Point[] pArray = new Point[myPts.Count];
			for(int iCount=0;iCount<myPts.Count;iCount++)
			{
				pArray[iCount]=(Point)myPts[iCount];
			}
			InPolygon = InsidePolygon(ref pArray,myPts.Count-1,CourseXY);
			return InPolygon;
		}

		//設定該Polygon之各點
		/** 
		 * Define all Point(GPS geodetic position) of the Polygon(GPS-Area)..
		 * \param spp Format => x1,y1;x2,y2;x3,y3
                 */
		public void SetPolygonPoint(string spp)
		{
			Array pp = spp.Split(';');			
			myPts.Clear();
			for(int iCount=0;iCount<=pp.Length-1;iCount++)
			{
				Array pppoint = pp.GetValue(iCount).ToString().Split(',');
				Point mp = new Point(int.Parse(pppoint.GetValue(0).ToString()),int.Parse(pppoint.GetValue(1).ToString()));
				myPts.Add(mp);
			}
		}

	}	
	
}
