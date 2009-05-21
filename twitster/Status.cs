
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using Jayrock.Json;

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
					//CreatedAt = Helpers.ParseDateTime(current.InnerText);
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
		
		public Status( JsonObject node, bool hackForSearchAPI )
		{
			// "created_at":
			//TODO: Parse stupid created date format. Damn you twitter
			//this.created_at = blah blah node["created_at"];
			
			Helpers.ParseJSONLong(node["id"], out this.id);
			this.text = (string)node["text"];
			
			this.source = (string)node["source"];
			
			// Fix the URL
			this.source = this.source.Replace("&lt;","<");
			this.source = this.source.Replace("&gt;",">");
			this.source = this.source.Replace("&quot;","\"");

			
			Helpers.ParseJSONBool(node["truncated"], out this.truncated);
			Helpers.ParseJSONLong(node["in_reply_to_status_id"], out this.in_reply_to_status_id);
			Helpers.ParseJSONLong(node["in_reply_to_user_id"], out this.in_reply_to_user_id);
			Helpers.ParseJSONBool(node["favorited"], out this.favorited);
			this.in_reply_to_screen_name = (string)node["in_reply_to_screen_name"];
			
			
			if( hackForSearchAPI )
			{
				Helpers.ParseJSONLong(node["to_user_id"], out this.in_reply_to_user_id);
				
				this.user = new User();
				
				long tmp;
				Helpers.ParseJSONLong(node["from_user_id"], out tmp);
				user.Id = tmp;
				user.Name = (string)node["from_user"];
				user.ProfileImageUrl = (string)node["profile_image_url"];

			}
			else
			{
				if( node["user"] != null )
				{
					this.user = new User( (JsonObject)node["user"] );
				}
			}
		}
	}
}
