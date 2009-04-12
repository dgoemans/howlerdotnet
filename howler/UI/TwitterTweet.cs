
using System;
using System.Collections;
using System.Collections.Generic;
using Qyoto;
using twitster;

namespace howler
{
	
	
	public class TwitterTweet : QSvgWidget
	{
		private QHBoxLayout layout;
		
		// Actual layout items
		private QLabel image;
		private QLabel text;

		public QLabel Image 
		{
			get {return image;	}
			set {image = value;}
		}		
		
		public TwitterTweet( QWidget parent )
			: base( parent )
		{
			this.Load("Resources/howler_tweet.svg");
			
			this.layout = new QHBoxLayout();
			this.SetLayout( layout );
			layout.Margin = 10;

			this.image = new QLabel();
			this.image.MinimumSize = new QSize( 64, 64 );
			this.image.MaximumSize = new QSize( 64, 64 );
			this.image.ScaledContents = true;
			
			this.text = new QLabel();
			this.text.WordWrap = true;

			this.layout.AddWidget( image );
			this.layout.AddWidget( text );

		}
		
		public void SetStatus( Status status )
		{
			string text = status.Text;
			
			string[] words = text.Split( new char[] {' '});
			
			for( int i = 0; i < words.Length; i++ )
			{
				if( words[i].Length > 2 && words[i][0] == '@' )
				{
					
					string newFirst = "@<a href=\"" + words[i] + "\">" + words[i].Substring(1) + "</a>";
					words[i] = newFirst;
				}
				
				QUrl url = new QUrl( words[i], QUrl.ParsingMode.StrictMode );
				

				if( url.IsValid() && !url.IsRelative() && words[i].Contains("//") )
				{
					string newFirst = "<a href=\"" + words[i] + "\">" + words[i] + "</a>";
					words[i] = newFirst;
				}
			}

			text = "<b><a href=\"@" + status.User.ScreenName + "\">" + status.User.ScreenName + "</a>: </b>";
			
			foreach( string s in words )
			{
				text += s+" ";
			}
			
			text += "<br>(posted from: " + status.Source + ")";
			
			this.text.Text = text;
			this.text.AdjustSize();

			QObject.Connect( this.text, SIGNAL("linkActivated(QString)"), this, SLOT("linkClicked(QString)") );
			
			this.image.Pixmap = new QPixmap("Resources/howler_icon.png");
		}
		
		
		protected new ITweetSignalEmitter Emit 
		{
			get {return (ITweetSignalEmitter) Q_EMIT;}
		}

		[Q_SLOT("imageLoaded(QNetworkReply*)")]
		public void ImageLoaded(QNetworkReply reply)
		{
			QPixmap pix = new QPixmap();
			string replyURL = reply.Url().ToString();
			pix.LoadFromData( reply.ReadAll(), replyURL.Substring( replyURL.Length-3 )  );
			this.image.Pixmap = pix;
		}
		
		
		[Q_SLOT("linkClicked(QString)")]
		public void LinkClicked(string link)
		{
			if( link[0] == '@' )
			{
				Emit.ReplyTo( link );
			}
			else
			{
				QDesktopServices.OpenUrl( new QUrl(link) );
			}
		}
	
	}
}
