using System.ComponentModel;
using Java.Beans;

namespace Xamarin.Forms.Platform.Android
{
	class NativeViewPropertyListener : Java.Lang.Object, IPropertyChangeListener, INotifyPropertyChanged
	{
		string _targetProperty
		{
			get;
			set;
		}

		public NativeViewPropertyListener(string propertyName)
		{
			_targetProperty = propertyName;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void PropertyChange(PropertyChangeEvent e)
		{
			if (e.PropertyName == _targetProperty)
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_targetProperty));
		}
	}
}