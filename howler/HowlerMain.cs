
using System;
using System.Collections;
using System.Collections.Generic;
using Qyoto;
using twitster;

namespace howler
{	
	public class HowlerMain : QWidget
	{
		Twitster twitterConnection;
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
			twitterConnection = new Twitster();

			twitterClient = new TwitterClient(this);
			twitterClient.TwitterConnection = twitterConnection;
			
			imTab = new QFrame( this );

			mainLayout = new QVBoxLayout(this);
			mainLayout.Margin = 1;
			
			tabs = new QTabWidget(this);
			tabs.AddTab( imTab, "Chat" );
			tabs.AddTab( twitterClient, "Twitter" );
			tabs.CurrentIndex = 1;
			
			statusBar = new QStatusBar(this);
			statusBar.AddPermanentWidget( twitterClient.StatusMessage );

			mainLayout.AddWidget( tabs );
			mainLayout.AddWidget( statusBar );
		}
		
		public void TwitterLogin( string username, string password )
		{
			twitterConnection.Connect( username, password );
			twitterClient.Refresh();
		}
	}
}
