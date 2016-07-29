using System;
using System.ComponentModel;
using System.Reflection;
using Xamarin.Forms.Internals;
using System.Linq;

namespace Xamarin.Forms
{
	class EventWrapper : INotifyPropertyChanged
	{
		string TargetProperty { get; set; }
		static readonly MethodInfo s_handlerinfo = typeof(EventWrapper).GetRuntimeMethods().Single(mi => mi.Name == "OnPropertyChanged" && mi.IsPublic == false);

		public EventWrapper(object target, string targetProperty, string updateSourceEventName)
		{
			TargetProperty = targetProperty;
			Delegate handlerDelegate = null;
			EventInfo updateSourceEvent = null;
			try
			{
				updateSourceEvent = target.GetType().GetRuntimeEvent(updateSourceEventName);
				handlerDelegate = s_handlerinfo.CreateDelegate(updateSourceEvent.EventHandlerType, this);
			}
			catch (Exception)
			{
				Log.Warning("EventWrapper", "Can not attach EventWrapper.");
			}
			if (updateSourceEvent != null && handlerDelegate != null)
				updateSourceEvent.AddEventHandler(target, handlerDelegate);
		}

		[Preserve]
		void OnPropertyChanged(object sender, EventArgs e)
		{
			PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(TargetProperty));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

