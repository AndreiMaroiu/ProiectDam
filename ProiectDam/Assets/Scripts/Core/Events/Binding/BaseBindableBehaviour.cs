using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Events.Binding
{
    public class BaseBindableBehaviour : MonoBehaviour
    {
        protected event Action OnDestroyEvent;

        public void Bind<T>(IBindable<T> source, Action<T> action)
        {
            source.OnValueChanged += action;

            OnDestroyEvent += () => source.OnValueChanged -= action;
        }

        public void Bind<T>(UnityEvent<T> source, UnityAction<T> action)
        {
            source.AddListener(action);

            OnDestroyEvent += () => source.RemoveListener(action);
        }

        public void Bind<T>(IBindSource<T> source, Action<T> action)
        {
            source.Bindable.OnValueChanged += action;

            OnDestroyEvent += () => source.Bindable.OnValueChanged -= action;
        }

        protected virtual void OnDestroy()
        {
            OnDestroyEvent?.Invoke();
        }
    }
}
