
using System;
using System.Collections.Generic;
using Qyoto;
using System.Security;

namespace howler
{
	
	
	public class SettingsWidget : QWidget
	{
		AddAccountWidget accountDialog;
		
		SettingsController controller;
		
		QGridLayout mainLayout;
		
		QListWidget accountsList;

		QPushButton btnAddAcc;
		QPushButton btnRemAcc;
		
		QPushButton btnSave;
		
		
		public SettingsWidget( SettingsController controller )
		{
			this.controller = controller;

			// Make the account dialog prevent clicks back to the settings
			accountDialog = new AddAccountWidget(controller);
			accountDialog.WindowModality = WindowModality.ApplicationModal;
			
			mainLayout = new QGridLayout( this );
			
			accountsList = new QListWidget(this);
			accountsList.IconSize = new QSize(48,48);
			
			mainLayout.AddWidget( accountsList, 1, 1, 1, 5 );
			
			btnAddAcc = new QPushButton(this);
			btnAddAcc.Text = "+";
			mainLayout.AddWidget( btnAddAcc, 2, 2 );

			btnRemAcc = new QPushButton(this);
			btnRemAcc.Text = "-";
			mainLayout.AddWidget( btnRemAcc, 2, 4 );

			btnSave = new QPushButton(this);
			btnSave.Text = "Save";
			mainLayout.AddWidget( btnSave, 3, 5 );

			Connect( btnSave, SIGNAL("clicked()"), this, SLOT("saveAndClose()") );
			Connect( btnAddAcc, SIGNAL("clicked()"), accountDialog, SLOT("show()") );
			Connect( btnRemAcc, SIGNAL("clicked()"), this, SLOT("removeSelectedAccount()") );
			Connect( accountDialog, SIGNAL("accountAdded()"), this, SLOT("accountDialogDone()") );
		}
		
		protected new ISettingsSignalEmitter Emit
		{
			get {return (ISettingsSignalEmitter) Q_EMIT;}
		}
		
		[Q_SLOT("accountDialogDone()")]
		private void AccountDialogDone()
		{
			accountDialog.Close();
			Sync();
		}
	
		[Q_SLOT("sync()")]
		public void Sync()
		{
			accountsList.Clear();
			
			foreach( AccountDetails acc in controller.GetAccounts() )
			{
				// TODO: This currently uses the name as what shows up in list, later this 
				// will change for flexibility reasons
				QListWidgetItem item = new QListWidgetItem( acc.Name );
				accountsList.AddItem(item);
			}
		}
		
		[Q_SLOT("saveAndClose()")]
		public void SaveAndClose()
		{
			Emit.SettingsSaved();
		}
		
		[Q_SLOT("removeSelectedAccount()")]
		public void RemoveSelectedAccount()
		{
			// TODO: this currently relies on the name being the text,
			// which is generally BAD. When the name/text thing changes, change this too
			if( accountsList.CurrentItem() != null )
			{
				controller.RemoveAccount( accountsList.CurrentItem().Text() );
			}
			
			Sync();
		}
	}
}
