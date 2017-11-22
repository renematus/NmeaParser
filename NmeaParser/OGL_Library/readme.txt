 Open GPS_LBS library.
-------------------------------------------------
 An open source library for GPS / LBS developers
-------------------------------------------------

Open GPS_LBS library is distributed under the GNU General Public License. 
Be sure to read it before using OGL-Library.


Tsung-Te, Wu.(p3p3, Taiwan)
<p3p3@mail2000.com.tw>
<http://ogl-lib.sourceforge.net/index.html>


What is it?
-----------
	"Open GPS / LBS library" is a C# base library project for developers who 
	would like to develop a GPS / LBS(Location Based Services) applications 
	on PC(Windows) or PocketPC(CE). This library provides some classes for programmers,
	such as NMEA0183 parser, convert GPS data(waypoints, routes, and tracks) to 
	GPX(the GPS eXchange Format) light-weight XML data format, WGS84 to TWD67(Taiwan) 
	coordinate transformation, and other useful functions for LBS application program development.

How to make DLL?
-----------------
	[1] You must be install Microsoft .NET Framework
	[2] csc /t:library /out:OGL_Library.dll *.cs

How do I Use the Class?
-------------------------
	[1] Using Microsoft Visual Studio .NET, and on "Solution Explorer" tab click right-button / Add Reference / Browse and Add OGL_Library.dll
	[2] In the code header,add "using OGL_Library;"
	[3] See more, check out the the documentation.

Bugs and suggestion to
------------------------
        p3p3 p3p3@mail2000.com.tw


End of Readme File