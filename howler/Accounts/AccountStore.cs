
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace howler
{
	
	[Serializable()]
	public class AccountStore : ISerializable
	{

		AccountDetails twitter;

		public AccountDetails Twitter 
		{	
			get {return twitter;}
			set {twitter = value;}
		}
		
		public AccountStore()
		{
		}
				
		public AccountStore(SerializationInfo info, StreamingContext ctxt)
		{
			this.twitter = (AccountDetails)info.GetValue("twitter", typeof(AccountDetails));
		}

		//Serialization function.
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			info.AddValue("twitter", this.twitter);
		}
	}
}