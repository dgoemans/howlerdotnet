
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace twitster
{
	
	
	public class Status
	{
		
		DateTime created_at;
		long id;
		string text;
		string source;
		bool truncated;
		long in_reply_to_status_id;
		long in_reply_to_user_id;
		bool favorited;
		string in_reply_to_screen_name;
		User user;

		public DateTime CreatedAt {
							get {
								return created_at;
							}
							set {
								created_at = value;
							}
						}		
		public bool Favorited {
					get {
						return favorited;
					}
					set {
						favorited = value;
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
	
		public string InReplyToScreenName {
						get {
							return in_reply_to_screen_name;
						}
						set {
							in_reply_to_screen_name = value;
						}
					}		
		public long InReplyToStatusId {
				get {
					return in_reply_to_status_id;
				}
				set {
					in_reply_to_status_id = value;
				}
			}	

		public long InReplyToUserId {
							get {
								return in_reply_to_user_id;
							}
							set {
								in_reply_to_user_id = value;
							}
						}		
		public string Source {
					get {
						return source;
					}
					set {
						source = value;
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
	
		public bool Truncated {
						get {
							return truncated;
						}
						set {
							truncated = value;
						}
					}
	
		public User User {
				get {
					return user;
				}
				set {
					user = value;
				}
			}	
		
		public Status( XmlNode node )
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
				case "source":
					Source = current.InnerText;
					break;
				case "truncated":
					bool.TryParse( current.InnerText, out truncated );
					break;
				case "in_reply_to_status_id":
					long.TryParse( current.InnerText, out in_reply_to_status_id );
					break;
				case "in_reply_to_user_id":
					long.TryParse( current.InnerText, out in_reply_to_user_id);
					break;
				case "favorited":
					bool.TryParse( current.InnerText, out favorited );
					break;
				case "in_reply_to_screen_name":
					InReplyToScreenName = current.InnerText; 
					break;
				case "user":
					User = new User( current );
					break;
				}
			}
		}
	}
}
