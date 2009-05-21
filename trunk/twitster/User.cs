
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using Jayrock.Json;

namespace twitster
{
	
	
	public class User
	{
		long id;
		string name;
		string screenName;
		string location;
		string description;
		string profileImageUrl;
		string url;
		bool isProtected;
		int followersCount;

		public string Description {
							get {
								return description;
							}
							set {
								description = value;
							}
						}		
		public int FollowersCount {
					get {
						return followersCount;
					}
					set {
						followersCount = value;
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
		public bool IsProtected {
				get {
					return isProtected;
				}
				set {
					isProtected = value;
				}
			}
		public string Url {
			get {
				return url;
			}
			set {
				url = value;
			}
		}
		public string ScreenName {
			get {
				return screenName;
			}
			set {
				screenName = value;
			}
		}
		public string ProfileImageUrl {
			get {
				return profileImageUrl;
			}
			set {
				profileImageUrl = value;
			}
		}
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}
		public string Location {
			get {
				return location;
			}
			set {
				location = value;
			}
		}

		public User(XmlNode node)
		{
			IEnumerator it = node.GetEnumerator();
			while( it.MoveNext() )
			{
				XmlNode current = (XmlNode)it.Current;
				switch( current.Name )
				{
				case "id":
					long.TryParse(current.InnerText, out id );
					break;
				case "name":
					Name = current.InnerText;
					break;
				case "screen_name":
					ScreenName = current.InnerText;
					break;
				case "description":
					Description = current.InnerText;
					break;
				case "location":
					Location = current.InnerText;
					break;
				case "profile_image_url":
					ProfileImageUrl = current.InnerText;
					break;
				case "url":
					Url = current.InnerText;
					break;
				case "protected":
					bool.TryParse( current.InnerText, out isProtected );
					break;
				case "followers_count":
					int.TryParse( current.InnerText, out followersCount );
					break;
				}
			}
		}
		
		public User()
		{
		}
		
		public User( JsonObject node )
		{
			Helpers.ParseJSONLong( node["id"], out this.id );
			this.name = (string)node["name"];
			this.screenName = (string)node["screen_name"];
			this.description = (string)node["description"];
			this.location = (string)node["location"];
			this.profileImageUrl = (string)node["profile_image_url"];
			this.url = (string)node["url"];
			Helpers.ParseJSONBool( node["protected"], out this.isProtected );
			Helpers.ParseJSONInt( node["followers_count"], out this.followersCount );
		}
	}
}
