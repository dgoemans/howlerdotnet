
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
		public Twitster()
		{
			connection = null;
		}
		
		public void Connect( string username, string password )
		{
			connection = new Connection(username, password);
		}
		
		public List<Status> GetFriendsTimeLine( uint pageNumber )
		{
			return connection.GetFriendsTimeline( pageNumber );
		}
		
		public List<Status> GetRepliesTimeLine( uint pageNumber )
		{
			return connection.GetRepliesTimeline( pageNumber );
		}
		
		public void UpdateStatus( string status )
		{
			if( connection == null ) throw new Exception("Null Connection");
			connection.Update( status );
		}
	}
}
