
using System;
using Qyoto;

namespace howler
{
	
	
	public interface ISettingsSignalEmitter : IQWidgetSignals
	{
		[Q_SIGNAL("settingsSaved()")]
		void SettingsSaved();
	}
}