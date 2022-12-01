using Core.Events.Binding;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Events
{
    [CreateAssetMenu(fileName = "New Button Event", menuName = "Scriptables/Events/ButtonEvent")]
    public sealed class ButtonEvent : ScriptableObject
    {
        private readonly BindableEvent<ButtonInfo> _showEvent = new();
        private readonly BindableEvent<ButtonInfo> _closeEvent = new();

        public event Action<ButtonInfo> OnShow
        {
            add => _showEvent.OnValueChanged += value;
            remove => _showEvent.OnValueChanged -= value;
        }

        public event Action<ButtonInfo> OnClose
        {
            add => _closeEvent.OnValueChanged += value;
            remove => _closeEvent.OnValueChanged -= value;
        }

        public void Show(string name, UnityAction action) => _showEvent.Invoke(new(name, action));

        public void Close(string name, UnityAction action) => _closeEvent.Invoke(new(name, action));

        public IBindable<ButtonInfo> OnShowBindable => _showEvent;
        public IBindable<ButtonInfo> OnCloseBindable => _closeEvent;

        public readonly struct ButtonInfo : IEquatable<ButtonInfo>
        {
            public ButtonInfo(string name, UnityAction action)
            {
                Name = name;
                Action = action;
            }

            public string Name { get; }
            public UnityAction Action { get; }

            public static bool operator ==(in ButtonInfo left, in ButtonInfo right)
                => left.Name == right.Name && left.Action == right.Action;

            public static bool operator !=(in ButtonInfo left, in ButtonInfo right)
                => left.Name != right.Name || left.Action != right.Action;

            public override int GetHashCode()
            {
                return HashCode.Combine(Name, Action);
            }

            public override bool Equals(object obj)
            {
                if (obj is ButtonInfo info)
                {
                    return this == info;
                }

                return false;
            }

            public bool Equals(ButtonInfo other)
            {
                return this == other;
            }
        }
    }
}
