
using System;
using Qyoto;
using agsXMPP;
using agsXMPP.protocol.client;

namespace howler
{
	
	
	public class ChatWidget : QWidget
	{
		string m_contactID;
		
		QListWidget chat;
		QLineEdit eChatEntry;
		QPushButton btnSend;
		
		QGridLayout layout;
		
		XmppClientConnection conn;

		public XmppClientConnection Conn {
					get {
						return conn;
					}
					set {
						conn = value;
					}
				}

		public string ContactID {
					get {
						return m_contactID;
					}
					set {
						m_contactID = value;
					}
				}		
		public ChatWidget( QWidget parent )
		:
		base( parent )
		{
			layout = new QGridLayout(this);
			
			chat = new QListWidget();
			layout.AddWidget( chat, 1,1,1,2 );
			
			eChatEntry = new QLineEdit();
			layout.AddWidget( eChatEntry, 2, 1 );
			
			btnSend = new QPushButton();
			btnSend.Text = "Send";
			layout.AddWidget( btnSend, 2, 2 );
			
			Connect( btnSend, SIGNAL("clicked()"), this, SLOT("send()") );
		}
		
		[Q_SLOT("send()")]
		public void Send()
		{
			Conn.Send( new Message(m_contactID, MessageType.chat, eChatEntry.Text));
			eChatEntry.Clear();
		}
	}
}
