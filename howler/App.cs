
using System;
//using Gtk;
using Qyoto;

namespace howler
{
	
	
	public class App
	{
		
		public static void Main( string[] args )
		{
			QApplication app = new QApplication( args );
			app.WindowIcon = new QIcon("Resources/howler_icon.png");
			app.ApplicationName = "Howler";
			app.QuitOnLastWindowClosed = true;
			
			HowlerMain win = new HowlerMain();
			win.SetWindowTitle( "Howler" );
			win.Show();
			QApplication.Exec();
		}
	}
}
