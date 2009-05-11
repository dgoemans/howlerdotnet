
using System;
using System.IO;
using System.Xml.Serialization;
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
			XmlSerializer serializer = new XmlSerializer(typeof(AccountStore));
			TextReader reader = new StreamReader("Accounts.xml");
			this.Store = (AccountStore)serializer.Deserialize(reader);
			reader.Close();
		}
		
		public void WriteData()
		{
			XmlSerializer serializer = new XmlSerializer(typeof(AccountStore));
			TextWriter writer = new StreamWriter("Accounts.xml");
			serializer.Serialize(writer, this.Store);
			writer.Close();
		}
	}
}
