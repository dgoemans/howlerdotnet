
using System;
using System.Xml;
using System.Web;
using System.Collections;
using System.Collections.Generic;

namespace twitster
{	
	public class Twitster
	{
		Connection connection;

		public Connection Connection 
		{
			get {return connection;}
		}
		
		
		public Twitster()
		{
			connection = null;
		}
		
		public void Connect( string username, string password )
		{
			connection = new Connection(username, password);
		}
	}
}
