using System;
using UnityEngine;

namespace Core.Events
{
    public class BaseBindableBehaviour : MonoBehaviour
    {
        protected event Action OnDestroy;

        public void Bind<T>(IBindable<T> source, Action<T> action)
        {
            source.OnValueChanged += action;

            OnDestroy += () => source.OnValueChanged -= action;
        }

        protected void Destroy()
        {
            OnDestroy?.Invoke();
        }
    }
}
