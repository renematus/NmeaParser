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

//==================================
//            �[�c
//
//  COMMAPI  �ʸ�Coredll.dll�q�TAPI
//  COMM     ����Ū��SerialPort
//  GPSReceive ����GPS�ʥ]����P�B�z
//==================================

using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace OGL_Library
{
	//GPS�s������
	/*! \class GPSReceive
	 *  \brief The class master function are enables access to GPS devices <BR>
	 *   and reading data. Provider method to parsing NMEA-0183 strings,<BR>
	 *   now supports $GPGGA, $GPGSA, $ GPRMC and $GPGSV. Developer can configure<BR>
	 *   from which message to extract GPS receiver information..
         */
	public class GPSReceive
	{
		private COMM comm;		

		//GGA�ʥ]���c
		/*! \struct MSG_GGA
		 *  \brief A NMEA0183 $GPGGA description of the struct type..
		 */
		public struct MSG_GGA
		{
			public string Receive_Time; /*!< Receive Time */ //�������ɶ��]�@�ɼзǮɡ^�A�榡�G�ɤ���
			public string Latitude; /*!< WGS84 Latitude */ //�n�סA�榡�G�פ�.��
			public char NS_Indicator; /*!< N or S */ //N�_�b�y�]S�h���n�b�y�^
			public string Longitude; /*!< Longitude */ //�g�סA�榡�G�פ�.��
			public char EW_Indicator; /*!< E or W */ //E�F�b�y�]W�h����b�y�^
			public int Position_Fix; /*!< GPS quality indicator */ //0 = Invalid, 1 = Valid SPS, 2 = Valid DGPS, 3 = Valid PPS
			public string Satellites_Used; /*!< Number of satellites in use */ //�ҨϥΤ��ìP��
			public string HDOP; /*!< Horizontal dilution of position */ //������׫��С]HDOP�^
			public string Altitude; /*!< Antenna altitude above/below mean sea level */ //�ѽu���ס]�����������^
			public char Altitude_Units; /*!< Antenna height unit */ //���]���ء^
			public string DGPS_Station_ID; /*!< DGPS Station ID */ //�򯸯���0000-1023
		}
		
		//RMC�ʥ]���c
		/*! \struct MSG_RMC
		 *  \brief A NMEA0183 $GPRMC description of the struct type..
		 */
		public struct MSG_RMC
		{
		}
		
		//GSV�ʥ]���c
		/*! \struct MSG_GSV
		 *  \brief A NMEA0183 $GPGSV description of the struct type..
		 */
		public struct MSG_GSV
		{
		}
		
		//GSA�ʥ]���c
		/*! \struct MSG_GSA
		 *  \brief A NMEA0183 $GPGSA description of the struct type..
		 */
		public struct MSG_GSA
		{
		}

		//�غc��
		/*!
		 * GPSReceive constructor.
		 * \param port The port that enables access to GPS device.
		 */
		public GPSReceive(int port)
		{			
			comm = new COMM();
			comm.CommPort = port;
			comm.BaudRate = 4800;			
			comm.DataBits = 8;
			comm.Parity = COMM.mParity.none;
			comm.StopBits = COMM.mStopBits.one;						
		}		

		//�}/��Port
		/*! 
		 * Open GPS device port or close<BR>
		 * Set PortOpen = true, will be open port..
		 * \return GPS device now state(Open / Close)
		 */
		public Boolean PortOpen
		{
			get
			{
				return comm.PortOpen;
			}
			set
			{
				if(value==true)
					comm.PortOpen = true;
				else
					comm.PortOpen = false;
			}			
		}

		//Ū���@���ʥ]
		/*!
		 *  Receive NMEA0183 message from GPS device
		 *  \return NMEA0183 format message that contain $GPGGA�B$GPRMC�B$GPGSA�B$GPGSV and other.<BR>
		 *  It determine by GPS device..
                 */
		public string ReceiveALL()
		{
			return comm.Input();
		}

		
		//GPS�ʥ]����(GGA)
		/*!
		 *  Receive NMEA0183 message from GPS device and extract $GPGGA data 
		 *  \return $GPGGA data of MSG_GGA struct type.
                 */
		public MSG_GGA ReceiveGGA()
		{			
			//$GPGGA,023528.572,2237.3625,N,12021.1512,E,1,06,1.4,70.1,M,,,,0000*35			
			
			MSG_GGA gga  = new MSG_GGA();
			gga.Position_Fix = 0;
			string[] str_gps = null;

			//�ھ�\n\r���Ϋʥ]			
			str_gps = comm.Input().Replace("\r","").Split('\n'); 

			//���X���Y��$GPGGA���ʥ]			
			for(int i=str_gps.GetUpperBound(0)-1;i>=0;i--)
			{
				
				string[] str_gga = null;
				str_gga = str_gps.GetValue(i).ToString().Split(',');				
				if(str_gga[0]=="$GPGGA")
				{
					gga.Receive_Time = str_gga[1];
					gga.Latitude = str_gga[2];
					gga.NS_Indicator =  Convert.ToChar(str_gga[3].Substring(0,1));
					gga.Longitude = str_gga[4];
					gga.EW_Indicator = Convert.ToChar(str_gga[5].Substring(0,1));
					gga.Position_Fix = int.Parse(str_gga[6]);
					gga.Satellites_Used = str_gga[7];
					gga.HDOP = str_gga[8];
					gga.Altitude = str_gga[9];
					gga.Altitude_Units = Convert.ToChar(str_gga[10].Substring(0,1));
					gga.DGPS_Station_ID = str_gga[11];
					break;
				}
			}					
			
			return gga;
		}

		//GPS�ʥ]����(RMC)
		/*!
		 *  Receive NMEA0183 message from GPS device and extract $GPRMC data 
		 *  \return $GPRMC data of MSG_RMC struct type.
                 */
		 /*public MSG_RMC ReceiveRMC()
		 {
		 }*/

		 //GPS�ʥ]����(GSV)
		/*!
		 *  Receive NMEA0183 message from GPS device and extract $GPGSV data 
		 *  \return $GPGSV data of MSG_GSV struct type.
                 */
		 /*public MSG_GSV ReceiveGSV()
		 {
		 }*/

		 //GPS�ʥ]����(GSA)
		/*!
		 *  Receive NMEA0183 message from GPS device and extract $GPGSA data 
		 *  \return $GPGSA data of MSG_GSA struct type.
                 */
		 /*public MSG_GSA ReceiveGSA()
		 {
		 }*/

		//GPS�ʥ]����(GSV)
		/*public int Receive_GSV()
		{					
			string[] str_gps = new string[1];
			int gsv = 0;

			//�ھ�\n\r���Ϋʥ]	
			str_gps = comm.Input().Replace("\r","").Split('\n'); 

			//���X���Y��$GPGGA���ʥ]			
			for(int i=0;i<=str_gps.GetUpperBound(0);i++)
			{
				string[] str_gsv = null;
				str_gsv = str_gps.GetValue(i).ToString().Split(',');					
				if(str_gsv[0]=="$GPGSV"&&str_gsv.GetUpperBound(0)>7)
				{
					if(str_gsv[7]!="")
						gsv = int.Parse(str_gsv[7]);
					else
						gsv = 0;
				}
			}					
			
			return gsv;
		}*/
	}

	//CF COMM�s������
	public class COMM
	{
		//�P�줸�ˬd�覡
		public enum mParity
		{
			none = 0,
			odd = 1,
			even = 2,
			mark = 3,
			space = 4
		};
		
		//����줸
		public enum mStopBits
		{
			one = 0,
			onePointFive = 1,
			two = 2
		};		
		
		//�w�]�Ѽ�
		private IntPtr COMM_Handle = (IntPtr)COMMAPI.INVALID_HANDLE_VALUE; //comm port handle
		private int COMM_Port = 1;                                         //comm port number
		private mParity COMM_Parity = mParity.none;                        //port parity
		private int COMM_DataBits = 8;					   //data bits
		private mStopBits COMM_StopBits = mStopBits.one;                   //stop bits
		private int COMM_BaudRate = 4800;                                  //port speed
		private int COMM_ReadBufferSize = 512;                             //default input buffer size
		private int COMM_WriteBufferSize = 512;                            //default Output buffer size
		private int COMM_Timeout = 1000;                                   //communication timeout
		private byte[] COMM_ReadBuffer;                                    //receive buffer


		//�]�w��ƶǰe�t�v
		public int BaudRate
		{
			get{return COMM_BaudRate;}
			set{COMM_BaudRate = value;}
		}

		//�]�w�ǰe���
		public int DataBits
		{
			get{return COMM_DataBits;}
			set{COMM_DataBits = value;}
		}

		//�]�wŪ������
		public int InputLen
		{
			get{return COMM_ReadBufferSize;}
			set{COMM_ReadBufferSize = value;}
		}

		//�]�w�P�줸�ˬd
		public mParity Parity
		{
			get{return COMM_Parity;}
			set{COMM_Parity = value;}
		}

		//�]�w����줸
		public mStopBits StopBits
		{
			get{return COMM_StopBits;}
			set{COMM_StopBits = value;}
		}

		//�]�wTimeOut�ɶ�
		public int Timeout
		{
			get{return COMM_Timeout;}
			set{COMM_Timeout = value;}
		}

		//�ˬd�q�TPort
		public int CommPort
		{
			get{return COMM_Port;}
			set
			{
				COMM_Port = value;
				if( COMM_Port<1 || COMM_Port>16 ) 
				{
					throw new CommPortException("Port Open Failure");
				}
				else
				{
					COMM_Handle = COMMAPI.CreateFile(
						"COM" + COMM_Port.ToString() + ":",
						COMMAPI.GENERIC_READ | COMMAPI.GENERIC_WRITE,
						0,
						IntPtr.Zero,
						COMMAPI.OPEN_EXISTING,
						0,
						IntPtr.Zero
					);
					if( COMM_Handle == (IntPtr)COMMAPI.INVALID_HANDLE_VALUE )
					{
						throw new CommPortException("Port is already in use");
					}
					else
					{
						COMM_Handle = Close((IntPtr)COMM_Handle);
					}

				}
			}
		}

		//Ū���q�T���
		public string Input()
		{
			Boolean ResultValue;
			ASCIIEncoding encoding = new ASCIIEncoding();
			uint lpNumberOfBytesRead = 0;
			COMMAPI.OVERLAPPED lpOverlapped;
			string ResultString = "";

			if(COMM_Handle != (IntPtr)COMMAPI.INVALID_HANDLE_VALUE)
			{
				COMM_ReadBuffer = new byte[COMM_ReadBufferSize];
				lpOverlapped = new COMMAPI.OVERLAPPED();
				ResultValue = COMMAPI.ReadFile(COMM_Handle,
							       COMM_ReadBuffer,
					                       (uint)COMM_ReadBufferSize,
					                        out lpNumberOfBytesRead,
								ref lpOverlapped);
				if(ResultValue==false)
				{
					COMM_Handle = (IntPtr)COMMAPI.INVALID_HANDLE_VALUE;
					throw new CommPortException("Error reading COM:" + COMM_Port);
				}
				else
				{					
					ResultString = encoding.GetString(COMM_ReadBuffer,0,COMM_ReadBuffer.Length);
				}				
			}			

			return ResultString;
			
		}

		//�}�ҳq�TPort
		public Boolean PortOpen
		{
			get
			{
				if(COMM_Handle==(IntPtr)COMMAPI.INVALID_HANDLE_VALUE)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			set
			{
				if(value==true)
					COMM_Handle = Open(COMM_Port,COMM_BaudRate,COMM_Parity,COMM_DataBits,COMM_StopBits,COMM_ReadBufferSize,COMM_Timeout);
				else
					COMM_Handle = Close(COMM_Handle);
			}
		}

		//�պAPort
		public IntPtr Open(int port, int baudrate, mParity parity, int databits, mStopBits stopbits, int readbuffersize, int timeout)
		{
			Boolean ResultValue;
			IntPtr commHandle = (IntPtr)COMMAPI.INVALID_HANDLE_VALUE;
			COMMAPI.COMMTIMEOUTS lpCommTimeouts = new COMMAPI.COMMTIMEOUTS();
			COMMAPI.DCB	lpDCB = new COMMAPI.DCB();
			
			if(port > 0)
			{
				//open comm port and get handle to device
				commHandle = COMMAPI.CreateFile(
					"COM" + port.ToString() + ":",
					COMMAPI.GENERIC_READ | COMMAPI.GENERIC_WRITE,
					0,
					IntPtr.Zero,
					COMMAPI.OPEN_EXISTING,
					0,
					IntPtr.Zero);

				//if open comm port is succeed
				if(commHandle != (IntPtr)COMMAPI.INVALID_HANDLE_VALUE)
				{
					//clear comm port rx and tx buffer
					ResultValue = COMMAPI.PurgeComm(commHandle,COMMAPI.PURGE_RXABORT | COMMAPI.PURGE_TXABORT);

					//get existing comm port configuration 
					ResultValue = COMMAPI.GetCommState(commHandle,ref lpDCB);

					//set communications parameters for comm port configuration
					lpDCB.fBinary = 1;
					lpDCB.fParity = 1;
					lpDCB.fOutxCtsFlow = 0;
					lpDCB.fOutxDsrFlow = 0;
					lpDCB.fDtrControl = 1;
					lpDCB.fDsrSensitivity = 0;
					lpDCB.fTXContinueOnXoff = 1;
					lpDCB.fOutX = 0;
					lpDCB.fInx = 0;
					lpDCB.fErrorChar = 0;
					lpDCB.fNull = 0;
					lpDCB.fRtsControl = 1;
					lpDCB.fAbortOnError = 0;
					lpDCB.StopBits = (byte)stopbits;
					lpDCB.ByteSize = (byte)databits;
					lpDCB.Parity = (byte)Parity;
					lpDCB.BaudRate = baudrate;

					ResultValue = COMMAPI.SetCommState(commHandle, ref lpDCB);

					//set comm port buffer size in number of bytes
					ResultValue = COMMAPI.SetupComm(commHandle, (uint)COMM_ReadBufferSize, (uint)COMM_WriteBufferSize);

					//set comm port timeouts milliseconds
					lpCommTimeouts.ReadIntervalTimeout = 0;
					lpCommTimeouts.ReadTotalTimeoutMultiplier = 0;
					lpCommTimeouts.ReadTotalTimeoutConstant = (uint)timeout;
					lpCommTimeouts.WriteTotalTimeoutMultiplier = 10;
					lpCommTimeouts.WriteTotalTimeoutConstant = 100;

					ResultValue = COMMAPI.SetCommTimeouts(commHandle, ref lpCommTimeouts);
				}
				else
				{
					commHandle = (IntPtr)COMMAPI.INVALID_HANDLE_VALUE;
				}
			}
			else //port <= 0
			{
				commHandle = (IntPtr)COMMAPI.INVALID_HANDLE_VALUE;
			}

			return commHandle;

		}

		//�����q�TPort
		public IntPtr Close(IntPtr handle)
		{
			Boolean ResultValue;
			if(handle != (IntPtr)COMMAPI.INVALID_HANDLE_VALUE)
			{
				ResultValue = COMMAPI.CloseHandle(handle);
				if(ResultValue==false)
				{
					throw new CommPortException("Unable to close serial port.");	
				}

			}

			return (IntPtr)COMMAPI.INVALID_HANDLE_VALUE;
		}	

	}
	
	//�ۭq�ҥ~����
	internal class CommPortException : Exception
	{
		public CommPortException(string msg) : base(msg) {}
	}

	internal class GPSException : Exception
	{
		public GPSException(string msg) : base(msg) {}
	}

	//COMM .NET API
	internal class COMMAPI
	{
		internal const UInt32 GENERIC_READ = 0x80000000;
		internal const UInt32 GENERIC_WRITE = 0x40000000;
		internal const UInt32 OPEN_EXISTING = 3;
		internal const Int32 INVALID_HANDLE_VALUE = -1;
		internal const UInt32 PURGE_TXABORT = 1;
		internal const UInt32 PURGE_RXABORT = 2;

		[StructLayout( LayoutKind.Sequential )]
		internal struct COMMTIMEOUTS 
		{
			internal UInt32 ReadIntervalTimeout;
			internal UInt32 ReadTotalTimeoutMultiplier;
			internal UInt32 ReadTotalTimeoutConstant;
			internal UInt32 WriteTotalTimeoutMultiplier;
			internal UInt32 WriteTotalTimeoutConstant;
		}

		[StructLayout( LayoutKind.Sequential )]
		internal struct DCB 
		{
			internal Int32 DCBlength;
			internal Int32 BaudRate;
			internal Int32 fBinary;
			internal Int32 fParity;
			internal Int32 fOutxCtsFlow;
			internal Int32 fOutxDsrFlow;
			internal Int32 fDtrControl;
			internal Int32 fDsrSensitivity;
			internal Int32 fTXContinueOnXoff;
			internal Int32 fOutX;
			internal Int32 fInx;
			internal Int32 fErrorChar;
			internal Int32 fNull;
			internal Int32 fRtsControl;
			internal Int32 fAbortOnError;
			internal Int32 fDummy2;
			internal Int16 wReserved;
			internal Int16 XonLim;
			internal Int16 XoffLim;
			internal byte ByteSize;
			internal byte Parity;
			internal byte StopBits;
			internal char XonChar;
			internal char XoffChar;
			internal char ErrorChar;
			internal char EofChar;
			internal char EvtChar;
			internal Int16 wReserved1;
		}

		[StructLayout( LayoutKind.Sequential )] 
		internal struct OVERLAPPED 
		{
			internal UIntPtr Internal;
			internal UIntPtr InternalHigh;
			internal UInt32 Offset;
			internal UInt32 OffsetHigh;
			internal IntPtr hEvent;
		}

		//COMM API

		[DllImport("Coredll.dll")]
		internal static extern Boolean GetCommState(IntPtr hFile, ref DCB lpDCB);

		[DllImport("Coredll.dll")]
		internal static extern Boolean SetCommState(IntPtr hFile, [In] ref DCB lpDCB);

		[DllImport("Coredll.dll")]
		internal static extern Boolean PurgeComm(IntPtr hFile, UInt32 dwFlags);

		[DllImport("Coredll.dll")]
		internal static extern Boolean SetCommTimeouts(IntPtr hFile, [In] ref COMMTIMEOUTS lpCommTimeouts);

		[DllImport("Coredll.dll")]
		internal static extern Boolean SetupComm(IntPtr hFile, UInt32 dwInQueue, UInt32 dwOutQueue);

		
		//FILE I/O API

		[DllImport("Coredll.dll")]
		internal static extern IntPtr CreateFile(String lpFileName, UInt32 dwDesiredAccess, UInt32 dwShareMode,
			IntPtr lpSecurityAttributes, UInt32 dwCreationDisposition, UInt32 dwFlagsAndAttributes,
			IntPtr hTemplateFile);

		[DllImport("Coredll.dll")]
		internal static extern Boolean CloseHandle(IntPtr hObject);

		[DllImport("Coredll.dll")]
		internal static extern Boolean ReadFile(IntPtr hFile, [Out] Byte[] lpBuffer, UInt32 nNumberOfBytesToRead,
			out UInt32 nNumberOfBytesRead, [In] ref OVERLAPPED lpOverlapped);		
	}
	
}
