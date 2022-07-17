using System;
using UnityEngine;

namespace Core.Events
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
                if (_value != value)
                {
                    _value = value;

                    OnValueChanged?.Invoke();
                }
            }
        }

        public static implicit operator int(IntEvent @event)
            => @event.Value;
    }
}
