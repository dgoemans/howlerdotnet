
using System;
using System.Collections.Generic;
using Qyoto;
using System.IO;

namespace howler
{
	
	
	public class SettingsController : QObject
	{
		SettingsWidget settingsView;
		SettingsManager settingsManager;
		
		public SettingsController()
		{
			settingsManager = new SettingsManager();
			settingsView = new SettingsWidget( this );
			
			Connect( settingsView, SIGNAL("settingsSaved()"), this, SLOT("saveAndClose()") );
		}
		protected new ISettingsSignalEmitter Emit
		{
			get {return (ISettingsSignalEmitter) Q_EMIT;}
		}
		
		public bool Init()
		{
			bool empty = true;
			if( File.Exists("Accounts.xml" ) )
			{
				settingsManager.Load();
				settingsView.Sync();
				empty = settingsManager.AccountManager.Store.Accounts.Count == 0;
			}
			
			return !empty;
			
		}
		
		// NAME IS UNIQUE!
		public bool AddAccount(string name, string service, string username, string password)
		{
			AccountDetails exists = GetAccount(name);
			if( exists != null ) return false;
			
			settingsManager.AccountManager.Store.AddAccount(name, username, password, service);
			settingsManager.Save();
			return true;
		}
		
		// NAME IS UNIQUE!
		public AccountDetails GetAccount(string name)
		{
			foreach( AccountDetails account in settingsManager.AccountManager.Store.Accounts )
			{
				if( account.Name == name )
					return account;
			}
			return null;
		}
		
		// NAME IS UNIQUE!
		public bool RemoveAccount(string name)
		{
			AccountDetails toRemove = GetAccount(name);
			if( toRemove != null )
			{
				settingsManager.AccountManager.Store.Accounts.Remove( toRemove );
				return true;
			}

			return false;
		}
		
		public List<AccountDetails> GetAccounts()
		{
			return settingsManager.AccountManager.Store.Accounts;
		}
		
		public void Show()
		{
			//TODO: Load data into the view?
			settingsView.Show();
		}
		
		public void Hide()
		{
			//TODO: Save view changes?
			settingsView.Hide();
		}
		
		[Q_SLOT("saveAndClose()")]
		public void SaveAndClose()
		{
			this.Hide();
			Emit.SettingsSaved();
		}
	}
}
