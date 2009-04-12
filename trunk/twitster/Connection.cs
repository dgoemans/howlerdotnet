using System.Net;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System;

namespace twitster
{

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

		private XmlDocument GetResponse( string url, WebMethod method )
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
				XmlDocument doc = new XmlDocument();
				// Stream is now a doc
				doc.Load(stream);
				return doc;

			}
			catch( WebException e )
			{
				Console.WriteLine( e.Message );
				Stream stream = e.Response.GetResponseStream();
				XmlDocument doc = new XmlDocument();
				doc.Load(stream);
				Console.WriteLine(doc.GetElementsByTagName("error").Item(0).InnerText);

				return null;
			}
			
		}

		public List<Status> GetFriendsTimeline( uint pageNumber )
		{
			List<Status> result = new List<Status>();
			
			XmlDocument doc = GetResponse( "http://twitter.com/statuses/friends_timeline.xml?page=" + pageNumber + "&count=6", WebMethod.GET );
			
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
		
		public List<Status> GetRepliesTimeline( uint pageNumber )
		{
			List<Status> result = new List<Status>();
			
			XmlDocument doc = GetResponse( "http://twitter.com/statuses/mentions.xml?page=" + pageNumber + "&count=6", WebMethod.GET );
			
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

		public bool Update( string text )
		{
			XmlDocument doc = GetResponse( "http://twitter.com/statuses/update.xml?status="+text+"&source=howler", WebMethod.POST );
			return ( doc != null );
		}
	}
}
