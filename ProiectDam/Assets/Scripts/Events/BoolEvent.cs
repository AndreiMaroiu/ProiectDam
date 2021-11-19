using System;
using UnityEditor;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "New Bool Event", menuName = "Scriptables/Events/Bool Event")]
    public class BoolEvent : ScriptableObject
    {
        public event Action OnValueChanged;

        private bool _value;

        public bool Value
        {
            get => _value;

            set
            {
                _value = value;

                OnValueChanged?.Invoke();
            }
        }

        public static implicit operator bool(BoolEvent @event)
            => @event.Value;
    }
}
