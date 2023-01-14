using Core.Events.Binding;
using System;

namespace Core
{
    public static class AwaitEventTrigger
    {
        public static void AwaitEvent<T>(this IBindable<T> bindable, Action<T> onEvent)
        {
            void helperOnEvent(T value)
            {
                onEvent(value);
                bindable.OnValueChanged -= helperOnEvent;
            }

            bindable.OnValueChanged += helperOnEvent;
        }

        public static void AwaitEvent<T>(this IBindSource<T> bindable, Action<T> onEvent)
        {
            bindable.Bindable.AwaitEvent(onEvent);
        }
    }
}
