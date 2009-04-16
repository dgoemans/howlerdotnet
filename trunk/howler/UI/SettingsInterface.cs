
using System;
using Qyoto;
using System.Security;

namespace howler
{
	
	
	public class SettingsInterface : QWidget
	{
		AccountManager accountManager;
		
		QGridLayout mainLayout;
		
		QLabel username;
		QLabel password;
		
		QLabel twitterLabel;
		QLineEdit twitterUser;
		QLineEdit twitterPass;
		
		QPushButton saveButton;
		
		
		public SettingsInterface()
		{
			accountManager = new AccountManager();
			mainLayout = new QGridLayout( this );
			
			username = new QLabel( this );
			username.Text = "Username";
			mainLayout.AddWidget( username, 0, 1 );
			
			password = new QLabel( this );
			password.Text = "Password";
			mainLayout.AddWidget( password, 0, 2 );
			
			twitterLabel = new QLabel(this);
			twitterLabel.Text = "Twitter";
			mainLayout.AddWidget( twitterLabel, 1, 0 );
			
			twitterUser = new QLineEdit(this);
			mainLayout.AddWidget( twitterUser, 1, 1 );
			
			twitterPass = new QLineEdit(this);
			twitterPass.echoMode = QLineEdit.EchoMode.Password;
			mainLayout.AddWidget( twitterPass, 1, 2 );
			
			saveButton = new QPushButton(this);
			saveButton.Text = "Save";
			mainLayout.AddWidget( saveButton, 2, 2 );
			
			Connect( saveButton, SIGNAL("clicked()"), this, SLOT("saveAndClose()") );
		}
		
		protected new ISettingsSignalEmitter Emit
		{
			get {return (ISettingsSignalEmitter) Q_EMIT;}
		}

		public AccountManager Accounts {
			get {
				return accountManager;
			}
			set {
				accountManager = value;
			}
		}
		
		[Q_SLOT("saveAndClose()")]
		public void saveAndClose()
		{
			accountManager.Store.Twitter = new AccountDetails( "twitter", this.twitterUser.Text, this.twitterPass.Text, "twitter" );
			accountManager.WriteData();
			Emit.SettingsSaved();
		}
	}
}
