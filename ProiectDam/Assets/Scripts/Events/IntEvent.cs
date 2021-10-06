using System;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "New Int Event", menuName = "Scriptables/Events/Int Event")]
    public class IntEvent : ScriptableObject
    {
        public event Action OnValueChanged;

        private int _value;

        public int Value
        {
            get => _value;

            set
            {
                _value = value;

                OnValueChanged?.Invoke();
            }
        }

        public static implicit operator int(IntEvent @event)
            => @event.Value;
    }
}
