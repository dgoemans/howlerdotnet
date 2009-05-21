using System.Net;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System;
using Jayrock.Json.Conversion;
using Jayrock.Json;

namespace twitster
{
	public class RequestState
	{
	  public HttpWebRequest request = null;
	  public HttpWebResponse response = null;
	}

	public class Connection
	{
		enum WebMethod
		{
			POST,
			GET
		};

		NetworkCredential credential;

		public Connection( string username, string password )
		{
			credential = new NetworkCredential( username, password );
		}

		private JsonObject GetJSONResponse( string url, WebMethod method )
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Credentials = credential;
			//request.PreAuthenticate = true;
			
			switch( method )
			{
			case WebMethod.POST:
				request.Method = "POST";
				break;
			case WebMethod.GET:
				request.Method = "GET";
				break;
			}

			try
			{
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream stream = response.GetResponseStream();
				TextReader reader = new StreamReader(stream);
				string content = reader.ReadToEnd();

				JsonObject result = (JsonObject)JsonConvert.Import( content );

				reader.Close();
				stream.Close();
				
				return result;

			}
			catch( WebException e )
			{
				Console.WriteLine( e.Message );
				return null;
			}
			
		}

		private XmlDocument GetXMLResponse( string url, WebMethod method )
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Credentials = credential;
			//request.PreAuthenticate = true;
			
			switch( method )
			{
			case WebMethod.POST:
				request.Method = "POST";
				break;
			case WebMethod.GET:
				request.Method = "GET";
				break;
			}

			try
			{

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				//TODO: ASync response so there isn't a delay in load
				Stream stream = response.GetResponseStream();
				XmlDocument result = new XmlDocument();
				result.Load(stream);
				stream.Close();
				
				return result;

			}
			catch( WebException e )
			{
				Console.WriteLine( e.Message );
				return null;
			}
			
		}
		
		public List<Status> GetSearchResponse( string query, uint count )
		{
			List<Status> result = new List<Status>();
			
			JsonObject json = GetJSONResponse( "http://search.twitter.com/search.json?count=" + count + "&q=" + query, WebMethod.GET );
			JsonArray array = (JsonArray)json["results"];
			Helpers.NullCheck(array);
			
			foreach( JsonObject obj in array )
			{
				result.Add( new Status(obj, true) );
			}
			
			return result;
			
		}

		public List<Status> GetFriendsTimeline( uint count )
		{
			List<Status> result = new List<Status>();
			
			XmlDocument doc = GetXMLResponse( "http://twitter.com/statuses/friends_timeline.xml?count=" + count, WebMethod.GET );
						
			if( doc != null )
			{

				// TODO: exchange this for something more efficient, like maybe check if a single status node exists
				if( doc.GetElementsByTagName("status").Count == 0 )
				{
					Console.WriteLine( "Invalid Response: Did not contain Status element" );
					return result;
				}
	
				// Enumerate the document and create the elements
				XmlNode head = doc.SelectSingleNode("statuses");
				if( head == null )
				{
					Console.WriteLine( "Invalid Response: Could not find statuses element" );
					return result;
				}
	
				IEnumerator it = head.GetEnumerator();
				while( it.MoveNext() )
				{
					XmlNode current = (XmlNode)it.Current;
					result.Add( new Status(current) );
				}
			}
			return result;
		}
		
		public List<Status> GetRepliesTimeline( uint count )
		{
			List<Status> result = new List<Status>();
			
			XmlDocument doc = GetXMLResponse( "http://twitter.com/statuses/mentions.xml?count=" + count, WebMethod.GET );
			
			if( doc != null )
			{

				// TODO: exchange this for something more efficient, like maybe check if a single status node exists
				if( doc.GetElementsByTagName("status").Count == 0 )
				{
					Console.WriteLine( "Invalid Response: Did not contain Status element" );
					return result;
				}
	
				// Enumerate the document and create the elements
				XmlNode head = doc.SelectSingleNode("statuses");
				if( head == null )
				{
					Console.WriteLine( "Invalid Response: Could not find statuses element" );
					return result;
				}
	
				IEnumerator it = head.GetEnumerator();
				while( it.MoveNext() )
				{
					XmlNode current = (XmlNode)it.Current;
					result.Add( new Status(current) );
				}
			}
			return result;
		}
		
		public List<Message> GetDirectsRecieved(uint count)
		{
			List<Message> result = new List<Message>();
			
			XmlDocument doc = GetXMLResponse( "http://twitter.com/statuses/direct_messages.xml?count=" + count, WebMethod.GET );
			
			if( doc != null )
			{

				// TODO: exchange this for something more efficient, like maybe check if a single status node exists
				if( doc.GetElementsByTagName("direct-message").Count == 0 )
				{
					Console.WriteLine( "Invalid Response: Did not contain direct-message element" );
					return result;
				}
	
				// Enumerate the document and create the elements
				XmlNode head = doc.SelectSingleNode("direct-messages");
				if( head == null )
				{
					Console.WriteLine( "Invalid Response: Could not find direct-messages element" );
					return result;
				}
	
				IEnumerator it = head.GetEnumerator();
				while( it.MoveNext() )
				{
					XmlNode current = (XmlNode)it.Current;
					result.Add( new Message(current) );
				}
			}
			return result;
		}

		public bool Update( string text )
		{
			XmlDocument doc = GetXMLResponse( "http://twitter.com/statuses/update.xml?status="+text+"&source=howler", WebMethod.POST );
			return ( doc != null );
		}
	}
}
