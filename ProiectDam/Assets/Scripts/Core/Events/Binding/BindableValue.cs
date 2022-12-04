using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Events.Binding
{
    [Serializable]
    public class BindableValue<T> : IBindable<T>
    {
        [SerializeField] private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value))
                {
                    return;
                }

                _value = value;
                OnValueChanged?.Invoke(value);
            }
        }

        public event Action<T> OnValueChanged;

        public Type ValueType => typeof(T);

        bool IBindable.Bind(IBindTarget target)
        {
            if (target is IBindTarget<T> t)
            {
                OnValueChanged += t.OnValueChange;
                return true;
            }

            return false;
        }

        bool IBindable.UnBind(IBindTarget target)
        {
            if (target is IBindTarget<T> t)
            {
                OnValueChanged -= t.OnValueChange;
                return true;
            }

            return false;
        }

        public void Invoke() => OnValueChanged.Invoke(Value);

        public static implicit operator T(BindableValue<T> value)
            => value.Value;

        public override string ToString()
            => Value.ToString();
    }
}
