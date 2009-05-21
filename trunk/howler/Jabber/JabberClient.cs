
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
		ChatWidget chatW;

		public JabberClient(QWidget parent)
		:
		base(parent)
		{
			chatW = new ChatWidget(null);
			
			contactsList = new QListWidget(this);
			
			jabberCon = new XmppClientConnection();
			jabberCon.Server = "gmail.com";
			jabberCon.ConnectServer = "talk.google.com";
			
			jabberCon.OnRosterItem += new XmppClientConnection.RosterHandler(OnContactChange);
			
			jabberCon.OnLogin += new ObjectHandler(OnLogin);
		
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
		
		public void OnLogin( object sender )
		{
			// TODO: setup contacts list refresh, etc
			chatW.ContactID = "dgoemans@gmail.com";
			chatW.Conn = jabberCon;
			chatW.Show();
		}
	}
}
