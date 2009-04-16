
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qyoto;

namespace howler
{	
	public class HowlerMain : QWidget
	{
		SettingsInterface settings;
//		Twitster twitterConnection;
		TwitterClient twitterClient;
		
		QStatusBar statusBar;
		
		QLayout mainLayout;
		QTabWidget tabs;
		QFrame imTab;

		public QStatusBar StatusBar 
		{
			get {return statusBar;	}
		}

		public HowlerMain()
			: base( (QWidget)null )
		{
			
			tabs = new QTabWidget(this);

			twitterClient = new TwitterClient(this);
//			twitterClient.TwitterConnection = twitterConnection;
			
			imTab = new QFrame( this );

			mainLayout = new QVBoxLayout(this);
			mainLayout.Margin = 1;
			
			
			//tabs.MinimumSize = new QSize( 360, 480 );
			tabs.AddTab( imTab, "Chat" );
			tabs.AddTab( twitterClient, "Twitter" );
			
			tabs.CurrentIndex = 1;
			
			statusBar = new QStatusBar(this);
			statusBar.AddPermanentWidget( twitterClient.StatusMessage );

			mainLayout.AddWidget( tabs );
			mainLayout.AddWidget( statusBar );
			
			
			//TODO: HACK: Move this to a much better place. Quick dirty hacks are not kewl.
			settings = new SettingsInterface();
			
			if( File.Exists("Accounts.dat" ) )
			{
				settings.Accounts.ReadData();
				InitClientWindow();
			}
			else
			{
				settings.Show();
			}
			
			Connect( settings, SIGNAL("settingsSaved()"), this, SLOT("initClientWindow()") );

		}
		
//		public void TwitterLogin( string username, string password )
//		{
//			twitterConnection.Connect( username, password );
//			twitterClient.Refresh();
//		}
		
		[Q_SLOT("initClientWindow()")]
		public void InitClientWindow()
		{
			settings.Hide();
			twitterClient.Connect( settings.Accounts.Store.Twitter.User, settings.Accounts.Store.Twitter.Pass );
		}
	}
}
