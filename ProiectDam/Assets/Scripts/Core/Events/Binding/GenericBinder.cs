using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Events.Binding
{
    public abstract class Binder<T> : MonoBehaviour
    {
        [SerializeField] private BaseEvent<T> _event;
        [SerializeField] private UnityEvent<T> _target;

        private Action<T> _action;

        protected virtual void Start()
        {
            _action = value => _target.Invoke(value);
            bool succes = _event.Bindable.Bind(_action);

            if (succes is false)
            {
                Debug.LogError("Could not bind source to target!");
            }

        }

        protected virtual void OnDestroy()
        {
            bool succes = _event.Bindable.UnBind(_action);

            if (succes is false)
            {
                Debug.LogError("Could not unbind source to target!");
            }
        }
    }
}
