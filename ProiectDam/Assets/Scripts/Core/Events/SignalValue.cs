using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Core.Events.Binding;

namespace Core
{
    public class SignalValue<T, ET> : IBindable<T>
    {
        private readonly Func<ET, T> _calculateValue;

        public SignalValue(Func<ET, T> calculateValue, IBindable<ET> bindable)
        {
            bindable.OnValueChanged += OnEventValueChanged;
            _calculateValue = calculateValue;
        }

        public Type ValueType => typeof(T);

        public event Action<T> OnValueChanged;
        public T Value { get; private set; }

        public bool Bind(IBindTarget target)
        {
            if (target is IBindTarget<T> bindable)
            {
                OnValueChanged += bindable.OnValueChange;
                return true;
            }

            return false;
        }

        public bool UnBind(IBindTarget target)
        {
            if (target is IBindTarget<T> bindable)
            {
                OnValueChanged -= bindable.OnValueChange;
                return true;
            }

            return false;
        }

        private void OnEventValueChanged(ET a)
        {
            Value = _calculateValue(a);
            OnValueChanged?.Invoke(Value);
        }
    }
}
