
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Web;
using System.Net;
using twitster;
using Qyoto;
using System.Threading;

namespace howler
{	
	public class TwitterClient : QSvgWidget
	{
		const int refreshTime = 5000*60;
		
		enum TwitterView
		{
			HOME,
			REPLIES,
			DIRECTS,
			SEARCH
		};
		
		List<ITwitterItem> statusList;
			
		const int statusesPerPage = 6;
		const int numPages = 6;
		
		QWidget tweetBoxFrame;
		QLayout tweetBoxLayout;
		QTextEdit tweetBox;
		QPushButton tweetButton;
		
		List<TwitterTweet> tweets;
		QLayout twitterLayout;
		
		QSvgWidget controlsFrame;
		QGridLayout controlsFrameLayout;
		QPushButton control_prevPage;
		QPushButton control_nextPage;
		QPushButton control_refresh;
		QPushButton control_home;
		QPushButton control_replies;
		QPushButton control_directs;
		
		QSvgWidget refresh_image;
		QSvgWidget next_image;
		QSvgWidget prev_image;
		QSvgWidget home_image;
		QSvgWidget replies_image;
		QSvgWidget directs_image;
		
		TwitterView currentView;
		
		QLabel statusMessage;
		
		uint currentTimelinePage;
		
		System.Timers.Timer refreshTimer;
		Thread threadReply;
		
		QWidget searchFrame;
		QHBoxLayout searchLayout;
		QPushButton searchButton;
		QLineEdit searchBox;

		Twitster twitter;
		public Twitster Twitter 
		{
			get {return twitter;	}
			set {twitter = value;}
		}

		public QLabel StatusMessage {
				get {
					return statusMessage;
				}
				set {
					statusMessage = value;
				}
			}	

		public TwitterClient(QWidget parent)
			:base( parent )
		{
			
			twitter = new Twitster();

			currentView = TwitterView.HOME;

			this.Load("Resources/howler_twitter_client.svg");

			currentTimelinePage = 1;

			twitterLayout = new QVBoxLayout(this);

			tweets = new List<TwitterTweet>();

			tweetBoxFrame = new QWidget( this );
			tweetBoxLayout = new QHBoxLayout( tweetBoxFrame );

			tweetBox = new QTextEdit(this);
			tweetBox.MaximumHeight = 80;
			this.tweetBox.AcceptRichText = false;

			tweetButton = new QPushButton(this);
			tweetButton.MinimumSize = new QSize( 64,64 );
			tweetButton.Text = "Tweet";
			Connect( tweetButton, SIGNAL("clicked()"), this, SLOT("tweet()") );
			Connect( tweetBox, SIGNAL("textChanged()"), this, SLOT("updateCharCount()") );

			tweetBoxLayout.AddWidget( tweetBox );
			tweetBoxLayout.AddWidget( tweetButton );

			twitterLayout.AddWidget( tweetBoxFrame );
			this.tweetBoxFrame.Hide();
			
			controlsFrame = new QSvgWidget();
			controlsFrameLayout = new QGridLayout(controlsFrame);
			
			control_prevPage = new QPushButton();
			control_prevPage.Flat = true;
			control_prevPage.MaximumSize = new QSize( 40, 40 );
			control_prevPage.MinimumSize = new QSize( 40, 40 );
			control_prevPage.AutoFillBackground = false;
			Connect( control_prevPage, SIGNAL("clicked()"), this, SLOT("prevPage()") );
			controlsFrameLayout.AddWidget( control_prevPage, 0, 0 );

			control_nextPage = new QPushButton();
			control_nextPage.Flat = true;
			control_nextPage.MaximumSize = new QSize( 40, 40 );
			control_nextPage.MinimumSize = new QSize( 40, 40 );
			control_nextPage.AutoFillBackground = false;
			Connect( control_nextPage, SIGNAL("clicked()"), this, SLOT("nextPage()") );
			controlsFrameLayout.AddWidget( control_nextPage, 0, 5 );
			
			control_home = new QPushButton();
			control_home.MaximumSize = new QSize( 40, 40 );
			control_home.MinimumSize = new QSize( 40, 40 );
			Connect( control_home, SIGNAL("clicked()"), this, SLOT("showHome()") );
			controlsFrameLayout.AddWidget( control_home, 0, 1 );

			
			control_replies = new QPushButton();
			control_replies.MaximumSize = new QSize( 40, 40 );
			control_replies.MinimumSize = new QSize( 40, 40 );
			Connect( control_replies, SIGNAL("clicked()"), this, SLOT("showReplies()") );
			controlsFrameLayout.AddWidget( control_replies, 0, 2 );
			
			control_directs = new QPushButton();
			control_directs.MaximumSize = new QSize( 40, 40 );
			control_directs.MinimumSize = new QSize( 40, 40 );
			Connect( control_directs, SIGNAL("clicked()"), this, SLOT("showDirects()") );
			controlsFrameLayout.AddWidget( control_directs, 0, 3 );
			
			control_refresh = new QPushButton();
			control_refresh.Flat = true;
			control_refresh.MaximumSize = new QSize( 40, 40 );
			control_refresh.MinimumSize = new QSize( 40, 40 );
			control_refresh.AutoFillBackground = false;
			controlsFrameLayout.AddWidget( control_refresh, 0, 4 );
			Connect( control_refresh, SIGNAL("clicked()"), this, SLOT("refreshAndReset()") );
			
			refresh_image = new QSvgWidget("Resources/howler_button_refresh.svg", control_refresh);
			refresh_image.Size = new QSize( 40, 40 );

			next_image = new QSvgWidget("Resources/howler_button_next.svg", control_nextPage);
			next_image.Size = new QSize( 40, 40 );

			prev_image = new QSvgWidget("Resources/howler_button_prev.svg", control_prevPage);
			prev_image.Size = new QSize( 40, 40 );

			home_image = new QSvgWidget("Resources/howler_button_home.svg", control_home);
			home_image.Size = new QSize( 40, 40 );

			directs_image = new QSvgWidget("Resources/howler_button_na.svg", control_directs);
			directs_image.Size = new QSize( 40, 40 );
			
			replies_image = new QSvgWidget("Resources/howler_button_replies.svg", control_replies);
			replies_image.Size = new QSize( 40, 40 );

			twitterLayout.AddWidget( controlsFrame );
			
			searchFrame = new QWidget(this);
			searchLayout = new QHBoxLayout(searchFrame);
			searchButton = new QPushButton(this);
			searchButton.Text = "search";
			
			searchBox = new QLineEdit(this);
			
			searchLayout.AddWidget( searchBox );
			searchLayout.AddWidget( searchButton );
			Connect( searchButton, SIGNAL("clicked()"), this, SLOT("search()") );

			twitterLayout.AddWidget( searchFrame );
			
			statusMessage = new QLabel(this);
			UpdateStatusText();
						
			refreshTimer = new System.Timers.Timer(refreshTime);
			refreshTimer.AutoReset = true;
			refreshTimer.Elapsed += refreshTimeout;
		}
		
		[Q_SLOT("showHome()")]
		public void ShowHome()
		{
			currentTimelinePage = 1;
			
			home_image.Load("Resources/howler_button_home_busy.svg");
			replies_image.Load("Resources/howler_button_replies.svg");
			directs_image.Load("Resources/howler_button_na.svg");

			currentView = TwitterView.HOME;
			Refresh();
		}
		
		[Q_SLOT("showReplies()")]
		public void ShowReplies()
		{
			currentTimelinePage = 1;
			
			home_image.Load("Resources/howler_button_home.svg");
			replies_image.Load("Resources/howler_button_replies_busy.svg");
			directs_image.Load("Resources/howler_button_na.svg");

			currentView = TwitterView.REPLIES;
			Refresh();
		}
		
		[Q_SLOT("showDirects()")]
		public void ShowDirects()
		{
			currentTimelinePage = 1;
			
			home_image.Load("Resources/howler_button_home.svg");
			replies_image.Load("Resources/howler_button_replies.svg");
			directs_image.Load("Resources/howler_button_na.svg");

			currentView = TwitterView.DIRECTS;
			Refresh();
		}
		
		[Q_SLOT("search()")]
		public void Search()
		{
			currentTimelinePage = 1;
			
			home_image.Load("Resources/howler_button_home.svg");
			replies_image.Load("Resources/howler_button_replies.svg");
			directs_image.Load("Resources/howler_button_na.svg");

			currentView = TwitterView.SEARCH;
			Refresh();
		}
		
		[Q_SLOT("search(QString)")]
		public void Search(string searchString)
		{
			this.searchBox.Text = searchString;
			
			currentTimelinePage = 1;
			
			home_image.Load("Resources/howler_button_home.svg");
			replies_image.Load("Resources/howler_button_replies.svg");
			directs_image.Load("Resources/howler_button_na.svg");

			currentView = TwitterView.SEARCH;
			Refresh();
		}

		
		[Q_SLOT("updateCharCount()")]
		public void UpdateCharCount()
		{
			UpdateStatusText();
		}
		
		[Q_SLOT("refreshAndReset()")]
		public void RefreshAndReset()
		{
			refresh_image.Load("Resources/howler_button_refresh_busy.svg");
			refresh_image.Update();
			refresh_image.Repaint();
			
			currentTimelinePage = 1;
			Refresh();
		}
		
		private void ClearTweets()
		{
			foreach( TwitterTweet t in tweets )
			{
				twitterLayout.RemoveWidget( t );
				t.Hide();
				t.Dispose();
			}

			tweets.Clear();
		}
		
		private void FillTweets()
		{
			int currentPos = (int)(currentTimelinePage-1)*statusesPerPage;
			if( currentPos >= statusList.Count ) currentPos = statusList.Count;
			int upperBound = currentPos + statusesPerPage;
			if( upperBound >= statusList.Count ) upperBound = statusList.Count;
			
			for( int i =  currentPos; i < upperBound; i++ )
			{
				TwitterTweet tweet = new TwitterTweet( this );
				tweet.SetItem( statusList[i] );
				
				tweets.Add( tweet );
				twitterLayout.AddWidget( tweet );
				
				GetImageFromURL( statusList[i].imageUrl(), ref tweet );
				
				Connect( tweet, SIGNAL("replyTo(QString)"), this, SLOT("startReply(QString)") );
				Connect( tweet, SIGNAL("search(QString)"), this, SLOT("search(QString)") );
			}
			
			this.Size = twitterLayout.MinimumSize();

			UpdateStatusText();
		}
		
		private void GetNewStatusList()
		{
			if( statusList != null )
			{
				statusList.Clear();
			}
			
			statusList = null;

			switch( currentView )
			{
			case TwitterView.HOME:
				statusList = twitter.Connection.GetFriendsTimeline( statusesPerPage*numPages );
				break;
			case TwitterView.REPLIES:
				statusList = twitter.Connection.GetRepliesTimeline( statusesPerPage*numPages );
				break;
			case TwitterView.DIRECTS:
				statusList = twitter.Connection.GetDirectsRecieved( statusesPerPage*numPages );
				break;
			case TwitterView.SEARCH:
				statusList = twitter.Connection.GetSearchResponse( Uri.EscapeDataString( this.searchBox.Text ), statusesPerPage*numPages );
				break;
			default:
				statusList = twitter.Connection.GetFriendsTimeline( statusesPerPage*numPages );
				break;
			}
			
			this.control_refresh.Click();

		}
		
		private void DisableControls()
		{
			this.control_directs.Enabled = false;
			this.control_home.Enabled = false;
			this.control_nextPage.Enabled = false;
			this.control_prevPage.Enabled = false;
			//this.control_refresh.Enabled = false;
			this.control_replies.Enabled = false;
			this.tweetButton.Enabled = false;
			this.searchButton.Enabled = false;
		}
		
		private void EnableControls()
		{
			this.control_directs.Enabled = true;
			this.control_home.Enabled = true;
			this.control_nextPage.Enabled = true;
			this.control_prevPage.Enabled = true;
			//this.control_refresh.Enabled = true;
			this.control_replies.Enabled = true;
			this.tweetButton.Enabled = true;
			this.searchButton.Enabled = true;
		}

		[Q_SLOT("refresh()")]
		public void Refresh()
		{
			if( threadReply != null && (threadReply.ThreadState == ThreadState.Stopped || threadReply.ThreadState == ThreadState.Aborted ) )
			{
				ClearTweets();
				FillTweets();
				threadReply = null;

				EnableControls();
			}
			else if( threadReply == null )
			{				
				DisableControls();

				threadReply = new Thread( new ThreadStart( GetNewStatusList ) );
				threadReply.Start();
			}

			refresh_image.Load("Resources/howler_button_refresh.svg");
		}
		
		[Q_SLOT("tweet()")]
		public void Tweet()
		{
			string escapedText = Uri.EscapeDataString( this.tweetBox.PlainText );
			escapedText.Replace(@"#",@"%23");

			this.twitter.Connection.Update( escapedText );
			this.tweetBox.Clear();
			
			Refresh();
		}
		
		[Q_SLOT("connect()")]
		public void Connect( string username, string password )
		{
			twitter.Connect( username, password );
			
			ShowHome();
			
			refreshTimer.Start();
			
			this.tweetBoxFrame.Show();

		}
		
		public void refreshTimeout(object sender,EventArgs eArgs)
		{
			control_refresh.Click();
		}
		
		public void GetImageFromURL( string url, ref TwitterTweet tweet )
		{
			QNetworkAccessManager access = new QNetworkAccessManager( this );
			
			Connect( access, SIGNAL("finished(QNetworkReply*)"), tweet, SLOT("imageLoaded(QNetworkReply*)") );
			Console.WriteLine( url + " -- " + url.Substring( url.Length-3 ) );
			access.Get( new QNetworkRequest( new QUrl( url ) ) );
			//Console.Write( "." );

		}		
		
		[Q_SLOT("startReply(QString)")]
		public void StartReply(string screenName)
		{
			this.tweetBox.SetFocus();
			this.tweetBox.InsertPlainText( screenName + " " );
		}

		[Q_SLOT("prevPage()")]
		public void PrevPage()
		{
			if( currentTimelinePage > 1 )
			{
				currentTimelinePage--;
				
				ClearTweets();
				FillTweets();
			}
		}
		
		[Q_SLOT("nextPage()")]
		public void NextPage()
		{
			if( currentTimelinePage < numPages )
			{
				currentTimelinePage++;

				ClearTweets();
				FillTweets();

			}
		}
		
		void UpdateStatusText()
		{
			int left = 140 - this.tweetBox.PlainText.Length;
			this.statusMessage.Text = left.ToString() + " - Page: " + currentTimelinePage + "/" + numPages;
			
			if( left < 0 )
			{
				this.statusMessage.SetStyleSheet( "color:#FF0000" );
			}
			else
			{
				this.statusMessage.SetStyleSheet( "color:#000000" );
			}

		}
	}
}
