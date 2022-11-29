using System;
using UnityEngine;
using Core.Events.Binding;

namespace Core.Events
{
    [CreateAssetMenu(fileName = "New Game Event", menuName = "Scriptables/Events/Game Event")]
    public class GameEvent : ScriptableObject, IBindSource
    {
        private readonly BindableEvent<object> _bindable = new();

        public IBindable Bindable => _bindable;

        public event Action<object> OnEvent
        {
            add => _bindable.OnValueChanged += value;
            remove => _bindable.OnValueChanged -= value;
        }

        public void Invoke(object sender) => _bindable.Invoke(sender);
    }
}
