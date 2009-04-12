
using System;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace howler
{
	
	[Serializable()]

	public class AccountDetails : ISerializable
	{
		string name;
		string user;
		SecureString pass;
		string service;
		
		
		public AccountDetails(string name, string user, SecureString pass, string service)
		{
			this.name = name;
			this.user = user;
			this.pass = pass;
			this.service = service;
		}
		
		public AccountDetails(SerializationInfo info, StreamingContext ctxt)
		{
			this.name = (String)info.GetValue("name", typeof(string));
			this.user = (String)info.GetValue("user", typeof(string));
			this.pass = (SecureString)info.GetValue("pass", typeof(SecureString));
			this.service = (String)info.GetValue("service", typeof(string));
		}
		        
		//Serialization function.
		
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			info.AddValue("name", this.name);
			info.AddValue("user", this.user);
			info.AddValue("pass", this.pass);
			info.AddValue("service", this.service);
		}
	}
}
