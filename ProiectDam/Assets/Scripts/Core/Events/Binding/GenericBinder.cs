using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Events.Binding
{
    public abstract class Binder<TEvent, T> : MonoBehaviour where TEvent : IBindSource<T>
    {
        [SerializeField] private TEvent _event;
        [SerializeField] private UnityEvent<T> _target;

        private Action<T> _action;

        protected virtual void Start()
        {
            _action = _target.Invoke;
            _event.Bindable.OnValueChanged += _action;
        }

        protected virtual void OnDestroy()
        {
            _event.Bindable.OnValueChanged -= _action;
        }
    }
}
