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
using System.Runtime.InteropServices;
using System.IO;

namespace OGL_Library
{
	/*! \class Sound
	 *  \brief The class provide plays *.wav and *.mp3 or another format sound on the WinCE or Windows. <BR>
	 *   It wrapped "PlaySound" API to using play sound and supports develop "Voice-Guided Tour"<BR>
	 *   application for PDA.. See <a href=http://msdn.microsoft.com/library/default.asp?url=/library/en-us/multimed/htm/_win32_playsound.asp target=_black>more</a>
	 */
	public class Sound
	{
		Byte[] m_rgbSound = null;

		/*!
		 * \enum PlaySoundType
		 *  PlaySoundType Sync, Async, NoDefault, Loop, Yield, NoWait.
		 */
		public enum PlaySoundType : int 
		{
			Sync = 0x0000, /*!< Synchronous playback of a sound event. */ 
			Async = 0x0001,  /*!< The sound is played asynchronously. */
			NoDefault = 0x0002,  /*!< No default sound event is used. If the sound cannot be found. */
			Loop = 0x0008,  /*!< The sound plays repeatedly until Play method is called again with the pszSound parameter set to NULL. You must also specify the SND_ASYNC flag to indicate an asynchronous sound event*/
			Yield = 0x0010,  /*!< The specified sound event will yield to another sound event that is already playing. If a sound cannot be played because the resource needed to generate that sound is busy playing another sound, the function immediately returns FALSE without playing the requested sound.*/
			NoWait = 0x00002000, /*!< If the driver is busy, return immediately without playing the sound.*/
		}
		
		private enum _PlaySoundFrom : int 
		{
			// maps to the WinCE PlaySound flags (SND_*)
			FromMemory = 0x0004,  
			FromFile = 0x00020000, 
			//FromAlias = 0x00010000, 
			//FromResource = 0x00040004  
		}
		
		/*!
		 * Play the sound by FileName
		 * \param strFileName File name.
		 */
		public Sound(String strFileName):this(new FileStream(strFileName, FileMode.Open))			
		{
			//if (new FileInfo(strFileName).Exists)				
		}
		
		/*!
		 * Play the sound by stream
		 * \param stream File Stream.
		 */
		public Sound(Stream stream)
		{
			m_rgbSound = new Byte[stream.Length];
			stream.Read(m_rgbSound, 0, (Int32)stream.Length);
		}
		
		/*!
		 * Start play the sound
		 */
		public void Play()
		{
			Play(PlaySoundType.Async /*| PlaySoundType.NoDefault*/);
		}
		
		/*!
		 * Flags for playing the sound.
		 * \param pst PlaySoundType enum.
		 */
		public void Play(PlaySoundType pst)
		{
			if (m_rgbSound != null)
				PlaySound(m_rgbSound, (int)_PlaySoundFrom.FromMemory | (int)pst);
		}
		
		/*!
		 * Stop sound.
		 */
		public void Stop()
		{
			StopAll();
		}
		
		public static void StopAll()
		{
			PlaySound(null, 0);
		}

		// no need for any lock because we are just calling Win32 API
		private static void PlaySound(Byte[] pszSound, int fdwSound)
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				W32_PlaySound(pszSound, IntPtr.Zero, fdwSound);
			else 
				WCE_PlaySound(pszSound, IntPtr.Zero, fdwSound);
		}
		
		[DllImport("winmm", EntryPoint="PlaySound", SetLastError=true)]
		private static extern bool W32_PlaySound(Byte[] pszSound, IntPtr hmod, int fdwSound);
		
		[DllImport("coredll", EntryPoint="PlaySound", SetLastError=true)]
		private static extern bool WCE_PlaySound(Byte[] pszSound, IntPtr hmod, int fdwSound);
	}
}
