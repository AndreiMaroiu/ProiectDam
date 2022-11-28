using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Events.Binding
{
    public abstract class Binder<T> : MonoBehaviour
    {
        [SerializeField] private ScriptableObject _event;
        [SerializeField] private UnityEvent<T> _target;

        private Action<T> _action;

        protected virtual void Start()
        {
            if (_event is IBindSource bindSource)
            {
                _action = value => _target.Invoke(value);
                bool succes = (bindSource.Bindable as IBindable<T>).Bind(_action);

                if (succes is false)
                {
                    Debug.LogError("Could not bind source to target!");
                }
            }
        }

        protected virtual void OnDestroy()
        {
            if (_event is IBindSource bindSource)
            {
                bool succes = (bindSource.Bindable as IBindable<T>).Bind(_action);

                if (succes is false)
                {
                    Debug.LogError("Could not bind source to target!");
                }
            }
        }
    }
}
