using System;
using UnityEngine;
using Core.Events.Binding;

namespace Core.Events
{
    public class BaseEvent<T> : ScriptableObject, IBindSource<T>
    {
        private readonly BindableEvent<T> _bindable = new();

        public IBindable<T> Bindable => _bindable;

        IBindable IBindSource.SimpleBindable => _bindable;

        public event Action<T> OnEvent
        {
            add => _bindable.OnValueChanged += value;
            remove => _bindable.OnValueChanged -= value;
        }

        public void Invoke(T args) => _bindable.Invoke(args);
    }
}
