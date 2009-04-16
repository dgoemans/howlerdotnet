
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace howler
{
	
	
	public class AccountManager
	{
		AccountStore store;

		public AccountStore Store 
		{
			get {return store;}
			set {store = value;}
		}		
		
		public AccountManager()
		{
			store = new AccountStore();
		}
		
		public void ReadData()
		{
			FileStream stream = File.Open("Accounts.dat", FileMode.Open);
			BinaryFormatter bformatter = new BinaryFormatter();
			//TODO: implement a serializable class that stores all the accounts 
			this.store = (AccountStore)bformatter.Deserialize(stream);
			stream.Close();
		}
		
		public void WriteData()
		{
			Stream stream = File.Open("Accounts.dat", FileMode.Create);
			BinaryFormatter bformatter = new BinaryFormatter();
			bformatter.Serialize(stream, this.store);
			stream.Close();
		}
	}
}
