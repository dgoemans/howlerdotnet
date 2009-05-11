
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace howler
{
	
	[Serializable()]
	public class AccountStore
	{

		List<AccountDetails> accounts;

		public List<AccountDetails> Accounts {
			get {
				return accounts;
			}
			set {
				accounts = value;
			}
		}

		public AccountStore()
		{
			accounts = new List<AccountDetails>();
		}
		
		public void AddAccount(string name, string user, string pass, string service)
		{
			accounts.Add( new AccountDetails(name,user,pass,service) );
		}
	}
}