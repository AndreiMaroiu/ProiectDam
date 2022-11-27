using System;
using UnityEngine;

namespace Core.Events
{
    public abstract class BaseEvent<T> : ScriptableObject, IBindSource
    {
        private BindableValue<T> _value = new();

        public event Action<T> OnValueChanged
        {
            add => _value.OnValueChanged += value;
            remove => _value.OnValueChanged += value;
        }

        public T Value
        {
            get => _value.Value;
            set => _value.Value = value;
        }

        public IBindable Bindable => _value;

        public static implicit operator T(BaseEvent<T> @event)
            => @event.Value;

        public override string ToString()
            => Value.ToString();
    }
}
