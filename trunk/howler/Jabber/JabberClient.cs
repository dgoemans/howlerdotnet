
using System;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.disco;
using agsXMPP.protocol.iq.roster;
using agsXMPP.exceptions;
using Qyoto;

namespace howler
{
	
	
	public class JabberClient : QWidget
	{
		
		XmppClientConnection jabberCon;
		QListWidget contactsList;

		public JabberClient(QWidget parent)
		:
		base(parent)
		{
			contactsList = new QListWidget(this);
			
			jabberCon = new XmppClientConnection();
			jabberCon.Server = "gmail.com";
			jabberCon.ConnectServer = "talk.google.com";
			
			jabberCon.OnRosterItem += new XmppClientConnection.RosterHandler(OnContactChange);
			
			jabberCon.OnLogin += delegate(object o) 
			{
				// TODO: setup contacts list refresh, etc
				Console.WriteLine("Connected: " + jabberCon.Authenticated );
				//jabberCon.Send(new Message("test@gmail.com", MessageType.chat, "Ping")); 
			};
			
			jabberCon.OnAuthError += delegate(object sender, agsXMPP.Xml.Dom.Element e) 
			{
				// TODO: alert message
				//Console.WriteLine("XMPP AuthError: " + e );
			};
			
			jabberCon.OnError += delegate(object sender, Exception ex) 
			{
				// TODO: alert message
				//Console.WriteLine("XMPP Error: " + ex.ToString() + " -- "  + ex.Message );
			};
		}
		
		public void Connect( string username, string password )
		{
			jabberCon.Open(username, password);
		}
		
		public void OnContactChange(object sender, RosterItem item)
		{
			Console.WriteLine( item + " called " + item.Jid  );
		}
	}
}
