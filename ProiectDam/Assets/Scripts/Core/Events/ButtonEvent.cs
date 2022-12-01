using Core.Events.Binding;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Events
{
    [CreateAssetMenu(fileName = "New Button Event", menuName = "Scriptables/Events/ButtonEvent")]
    public class ButtonEvent : ScriptableObject
    {
        private readonly BindableEvent<ButtonInfo> _showEvent = new();
        private readonly BindableEvent<object> _closeEvent = new();

        public event Action<ButtonInfo> OnShow
        {
            add => _showEvent.OnValueChanged += value;
            remove => _showEvent.OnValueChanged -= value;
        }

        public event Action<object> OnClose
        {
            add => _closeEvent.OnValueChanged += value;
            remove => _closeEvent.OnValueChanged -= value;
        }

        public void Show(string name, UnityAction action) => _showEvent.Invoke(new(name, action));

        public void Close(object sender = null) => _closeEvent.Invoke(sender);

        public IBindable<ButtonInfo> OnShowBindable => _showEvent;
        public IBindable<object> OnCloseBindable => _closeEvent;

        public readonly struct ButtonInfo
        {
            public ButtonInfo(string name, UnityAction action)
            {
                Name = name;
                Action = action;
            }

            public string Name { get; }
            public UnityAction Action { get; }
        }
    }
}
