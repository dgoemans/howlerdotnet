
using System;

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
	}
}
