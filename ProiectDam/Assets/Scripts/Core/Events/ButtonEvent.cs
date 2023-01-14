using Core.Events.Binding;
using Core.Mappers;
using System;
using UnityEngine;

namespace Core.Events
{
    [CreateAssetMenu(fileName = "New Button Event", menuName = "Scriptables/Events/ButtonEvent")]
    public sealed class ButtonEvent : ScriptableObject
    {
        private readonly BindableEvent<IButtonModel> _showEvent = new();
        private readonly BindableEvent<IButtonModel> _closeEvent = new();
        private readonly BindableEvent<GameObject> _pressEvent = new();

        public event Action<IButtonModel> OnShow
        {
            add => _showEvent.OnValueChanged += value;
            remove => _showEvent.OnValueChanged -= value;
        }

        public event Action<IButtonModel> OnClose
        {
            add => _closeEvent.OnValueChanged += value;
            remove => _closeEvent.OnValueChanged -= value;
        }

        public event Action<GameObject> OnPress
        {
            add => _pressEvent.OnValueChanged += value;
            remove => _pressEvent.OnValueChanged -= value;
        }

        public void Show(IButtonModel info) => _showEvent.Invoke(info);

        public void Close(IButtonModel info) => _closeEvent.Invoke(info);

        public void Press(GameObject sender) => _pressEvent.Invoke(sender);

        public IBindable<IButtonModel> OnShowBindable => _showEvent;
        public IBindable<IButtonModel> OnCloseBindable => _closeEvent;
        public IBindable<GameObject> OnPressBindable => _pressEvent;
    }
}
