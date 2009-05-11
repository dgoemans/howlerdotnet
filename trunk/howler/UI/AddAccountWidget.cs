
using System;
using Qyoto;

namespace howler
{
	
	
	public class AddAccountWidget : QWidget
	{
		SettingsController controller;
		
		QComboBox cmAccountType;
		
		QLabel lUsername;
		QLabel lPassword;
		
		QLineEdit eUsername;
		QLineEdit ePassword;
		
		QPushButton btnOk;
		QPushButton btnCancel;
		
		QGridLayout pageLayout;

		public AddAccountWidget( SettingsController controller )
		{
			this.controller = controller;

			cmAccountType = new QComboBox( this );
			cmAccountType.AddItem("Google Talk");
			cmAccountType.AddItem("Twitter");
			
			lUsername = new QLabel(this);
			lUsername.Text = "Username";
			
			lPassword = new QLabel(this);
			lPassword.Text = "Password";
			
			eUsername = new QLineEdit(this);
			
			ePassword = new QLineEdit(this);
			ePassword.echoMode = QLineEdit.EchoMode.Password;
			
			btnOk = new QPushButton(this);
			btnOk.Text = "Ok";
			//Connect( btnOk, SIGNAL("clicked()"), this 
			
			btnCancel = new QPushButton(this);
			btnCancel.Text = "Cancel";

			pageLayout = new QGridLayout(this);
			pageLayout.AddWidget( cmAccountType, 0,0, 1,2 );
			
			pageLayout.AddWidget( lUsername, 1,0 );
			pageLayout.AddWidget( eUsername, 1,1, 1,2 );

			pageLayout.AddWidget( lPassword, 2,0 );
			pageLayout.AddWidget( ePassword, 2,1, 1,2 );
			
			pageLayout.AddWidget( btnOk, 4,1 );
			pageLayout.AddWidget( btnCancel, 4,2 );
			
			Connect(btnCancel, SIGNAL("clicked()"), this, SLOT("dismiss()") );
			Connect(btnOk, SIGNAL("clicked()"), this, SLOT("save()") );
		}
		
		protected new ISettingsSignalEmitter Emit
		{
			get {return (ISettingsSignalEmitter) Q_EMIT;}
		}
		
		[Q_SLOT("dismiss()")]
		public void Dismiss()
		{
			cmAccountType.CurrentIndex = 0;
			eUsername.Text = "";
			ePassword.Text = "";
			Emit.AccountAdded();
		}
		
		[Q_SLOT("save()")]
		public void Save()
		{
			controller.AddAccount( cmAccountType.CurrentText, cmAccountType.CurrentText, eUsername.Text, ePassword.Text );
			Emit.AccountAdded();
		}
	}
}
