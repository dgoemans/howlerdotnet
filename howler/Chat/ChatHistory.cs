
using System;
using System.Collections.Generic;


namespace howler
{
	public struct ChatItem
	{
		public string senderID;
		public string chatMessage;
		public DateTime timeStamp;
	}
	
	public class ChatHistory
	{
		List<ChatItem> chatItems;
		string senderID;
		string receiverID;
		
		public ChatHistory(string senderID, string receiverID)
		{
			chatItems = new List<ChatItem>();
		}
		
		public void AddMessage( string message )
		{
			ChatItem newItem = new ChatItem();
			newItem.senderID = this.senderID;
			newItem.chatMessage = message;
			newItem.timeStamp = DateTime.Now;
			
			chatItems.Add(newItem);
		}
		
		public List<ChatItem> GetLatestMessages( int count )
		{
			return chatItems.GetRange( chatItems.Count - count - 1, count );
		}
	}
}
