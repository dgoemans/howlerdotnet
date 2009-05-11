
using System;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace howler
{
	
	[Serializable()]
	public class AccountDetails
	{
		string name;
		string user;
		string pass;
		string service;

		public string Name {
					get {
						return name;
					}
					set {
						name = value;
					}
				}

		public string Pass {
					get {
						return pass;
					}
					set {
						pass = value;
					}
				}

		public string Service {
					get {
						return service;
					}
					set {
						service = value;
					}
				}

		public string User {
					get {
						return user;
					}
					set {
						user = value;
					}
				}		
		
		public AccountDetails(string name, string user, string pass, string service)
		{
			this.name = name;
			this.user = user;
			this.pass = pass;
			this.service = service;
		}
		
		public AccountDetails()
		{
		}
		

	}
}
