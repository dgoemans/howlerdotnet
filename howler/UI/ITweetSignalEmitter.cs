
using System;
using Qyoto;

namespace howler
{
	
	
	public interface ITweetSignalEmitter : IQWidgetSignals
	{
		[Q_SIGNAL("replyTo(QString)")]
		void ReplyTo(string name);
		
		[Q_SIGNAL("search(QString)")]
		void Search(string name);

	}
}
