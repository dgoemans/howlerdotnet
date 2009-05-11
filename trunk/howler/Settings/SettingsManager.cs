
using System;

namespace howler
{
	
	
	public class SettingsManager
	{
		AccountManager accountManager;

		public AccountManager AccountManager {
			get {
				return accountManager;
			}
			set {
				accountManager = value;
			}
		}
		
		public SettingsManager()
		{
			accountManager = new AccountManager();
		}
		
		public void Save()
		{
			try
			{
				accountManager.WriteData();
			}
			catch( Exception e )
			{
				Console.WriteLine("Error writing settings: " + e );
			}
		}
		
		public void Load()
		{
			try
			{
				accountManager.ReadData();
			}
			catch( Exception e )
			{
				Console.WriteLine("Error reading settings: " + e );
			}
		}
		
	}
}
