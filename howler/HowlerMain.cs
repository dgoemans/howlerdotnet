
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qyoto;


namespace howler
{	
	public class HowlerMain : QWidget
	{
		SettingsController settings;
		JabberClient jabberClient;
		TwitterClient twitterClient;
		
		QStatusBar statusBar;
		
		QLayout mainLayout;
		QTabWidget tabs;
		
		QPushButton btnSettings;
		

		public QStatusBar StatusBar 
		{
			get {return statusBar;	}
		}

		public HowlerMain()
			: base( (QWidget)null )
		{
		
			tabs = new QTabWidget(this);

			twitterClient = new TwitterClient(this);
			jabberClient = new JabberClient(this);
			
			mainLayout = new QVBoxLayout(this);
			mainLayout.Margin = 1;
			
			
			//tabs.MinimumSize = new QSize( 360, 480 );
			tabs.AddTab( jabberClient, "Chat" );
			tabs.AddTab( twitterClient, "Twitter" );
			
			tabs.CurrentIndex = 1;
			
			statusBar = new QStatusBar(this);
			statusBar.AddPermanentWidget( twitterClient.StatusMessage );

			btnSettings = new QPushButton(this);
			btnSettings.Text = "...";
			Connect( btnSettings, SIGNAL("clicked()"), this, SLOT("showSettings()") );
			
			mainLayout.AddWidget( tabs );
			mainLayout.AddWidget(btnSettings);
			mainLayout.AddWidget( statusBar );
			
			
			//TODO: HACK: Move this to a much better place. Quick dirty hacks are not kewl.
			settings = new SettingsController();
			
			bool hasAccounts = settings.Init();
			
			if( !hasAccounts )
			{
				settings.Show();
			}
			else
			{
				InitClientWindow();
			}
			
			Connect( settings, SIGNAL("settingsSaved()"), this, SLOT("initClientWindow()") );

		}
		
		[Q_SLOT("showSettings()")]
		public void ShowSettings()
		{
			settings.Show();			
		}
		
//		public void TwitterLogin( string username, string password )
//		{
//			twitterConnection.Connect( username, password );
//			twitterClient.Refresh();
//		}
		
		[Q_SLOT("initClientWindow()")]
		public void InitClientWindow()
		{
			AccountDetails account = settings.GetAccount("Twitter");
			if( account != null )
			{
				twitterClient.Connect(account.User, account.Pass);
			}
			
			account = settings.GetAccount("Google Talk");
			if( account != null )
			{
				//jabberClient.Connect(account.User, account.Pass);
			}
		}
	}
}
