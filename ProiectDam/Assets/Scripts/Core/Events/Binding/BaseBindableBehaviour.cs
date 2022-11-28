using System;
using UnityEngine;

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

        protected virtual void OnDestroy()
        {
            OnDestroyEvent?.Invoke();
        }
    }
}
