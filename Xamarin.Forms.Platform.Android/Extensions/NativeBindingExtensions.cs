using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Java.Beans;

namespace Xamarin.Forms.Platform.Android
{
	public static class NativeBindingExtensions
	{
		static ConditionalWeakTable<global::Android.Views.View, PropertyChangeSupport> _viewsWithPropertyChangeSupport { get; } = new ConditionalWeakTable<global::Android.Views.View, PropertyChangeSupport>();

		public static void SetBinding(this global::Android.Views.View view, string propertyName, BindingBase binding, string eventSourceName)
		{
			NativeBindingHelpers.SetBinding(view, propertyName, binding, eventSourceName);
		}

		public static void SetBinding(this global::Android.Views.View view, string propertyName, BindingBase binding)
		{
			var nativePropertyListener = SubscribeTwoWayIfNeeded(view, propertyName, binding);
			NativeBindingHelpers.SetBinding(view, propertyName, binding, nativePropertyListener);
		}

		public static void SetValue(this global::Android.Views.View target, BindableProperty targetProperty, object value)
		{
			NativeBindingHelpers.SetValue(target, targetProperty, value);
		}

		public static void SetBindingContext(this global::Android.Views.View target, object bindingContext, Func<global::Android.Views.View, IEnumerable<global::Android.Views.View>> getChildren = null)
		{
			NativeBindingHelpers.SetBindingContext(target, bindingContext, getChildren);
		}

		static NativeViewPropertyListener SubscribeTwoWayIfNeeded(global::Android.Views.View view, string propertyName, BindingBase binding)
		{
			if (binding.Mode != BindingMode.TwoWay)
				return null;
			PropertyChangeSupport pcSupport;
			if (!_viewsWithPropertyChangeSupport.TryGetValue(view, out pcSupport))
			{
				pcSupport = new PropertyChangeSupport(view);
				_viewsWithPropertyChangeSupport.Add(view, pcSupport);
			}
			if (pcSupport.HasListeners(propertyName))
				return null;
			var nativePropertyListener = new NativeViewPropertyListener(propertyName);
			pcSupport.AddPropertyChangeListener(nativePropertyListener);

			return nativePropertyListener;
		}
	}
}

