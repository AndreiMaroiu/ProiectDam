using System;
using UnityEngine;
using Core.Events.Binding;

namespace Core.Events
{
    [CreateAssetMenu(fileName = "New Game Event", menuName = "Scriptables/Events/Game Event")]
    public sealed class GameEvent : ScriptableObject, IBindSource<object>
    {
        private readonly BindableEvent<object> _bindable = new();

        public IBindable<object> Bindable => _bindable;

        IBindable IBindSource.SimpleBindable => _bindable;

        public event Action<object> OnEvent
        {
            add => _bindable.OnValueChanged += value;
            remove => _bindable.OnValueChanged -= value;
        }

        public void Invoke(object sender) => _bindable.Invoke(sender);
    }
}
