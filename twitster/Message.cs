
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using Jayrock.Json;

namespace twitster
{
	
	
	public class Message
	{
		long id;
		long sender_id;
		string text;
		long recipient_id;
		DateTime created_at;
		string sender_screen_name;
		string recipient_screen_name;
		User sender;
		User recipient;	
		
		public DateTime CreatedAt {
					get {
						return created_at;
					}
					set {
						created_at = value;
					}
				}		
		public long Id {
			get {
				return id;
			}
			set {
				id = value;
			}
		}
	
		public User Recipient {
						get {
							return recipient;
						}
						set {
							recipient = value;
						}
					}		
		public long RecipientId {
				get {
					return recipient_id;
				}
				set {
					recipient_id = value;
				}
			}	

		public string RecipientScreenName {
					get {
						return recipient_screen_name;
					}
					set {
						recipient_screen_name = value;
					}
				}		
		public User Sender {
			get {
				return sender;
			}
			set {
				sender = value;
			}
		}
	
		public string Text {
			get {
				return text;
			}
			set {
				text = value;
			}
		}
		public string SenderScreenName {
			get {
				return sender_screen_name;
			}
			set {
				sender_screen_name = value;
			}
		}
		public long SenderId {
				get {
					return sender_id;
				}
				set {
					sender_id = value;
				}
			}	
		
		public Message()
		{
		}
		
		
		public Message( XmlNode node )
		{
			IEnumerator it = node.GetEnumerator();
			while( it.MoveNext() )
			{
				XmlNode current = (XmlNode)it.Current;
				switch( current.Name )
				{
				case "created_at":
					//TODO: Parse stupid created date format. Damn you twitter
					CreatedAt = Helpers.ParseDateTime(current.InnerText);
					break;
				case "id":
					long.TryParse(current.InnerText, out id );
					break;
				case "text":
					Text = current.InnerText;
					break;
				case "sender_id":
					long.TryParse( current.InnerText, out recipient_id );
					break;
				case "recipient_id":
					long.TryParse( current.InnerText, out sender_id );
					break;
				case "recipient_screen_name":
					RecipientScreenName = current.InnerText; 
					break;
				case "sender_screen_name":
					SenderScreenName = current.InnerText; 
					break;
				case "sender":
					Sender = new User( current );
					break;
				case "recipient":
					Recipient = new User( current );
					break;

				}
			}
		}
		
		public Message( JsonObject node )
		{
			// TODO: FIX STUPID CREATED AT CRAP
			//this.created_at;
			Helpers.ParseJSONLong( node["id"], out this.id );
			
			this.text = (string)node["text"];
			
			Helpers.ParseJSONLong( node["sender_id"], out this.sender_id );
			Helpers.ParseJSONLong( node["recipient_id"], out this.recipient_id );
				
			this.sender_screen_name = (string)node["sender_screen_name"];
			this.recipient_screen_name = (string)node["recipient_screen_name"];
			
			this.sender = new User( (JsonObject) node["sender"] );
			this.recipient = new User( (JsonObject) node["recipient"] );
		}
	}
}
