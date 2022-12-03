using Core.Events.Binding;
using System;
using UnityEngine;

namespace Core.Events
{
    public abstract class BaseValueEvent<T> : ScriptableObject, IBindSource<T>
    {
        private readonly BindableValue<T> _value = new();

        public event Action<T> OnValueChanged
        {
            add => _value.OnValueChanged += value;
            remove => _value.OnValueChanged -= value;
        }

        public T Value
        {
            get => _value.Value;
            set => _value.Value = value;
        }

        IBindable IBindSource.SimpleBindable => _value;

        public IBindable<T> Bindable => _value;

        public static implicit operator T(BaseValueEvent<T> @event)
            => @event.Value;

        public override string ToString()
            => Value.ToString();
    }
}
