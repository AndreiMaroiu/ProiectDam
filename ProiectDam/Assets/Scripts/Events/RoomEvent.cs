using Core;
using System;
using UnityEngine;

namespace Events
{
    public class RoomEvent : ScriptableObject
    {
        public event Action OnValueChanged;

        private Room _value;

        public Room Value
        {
            get => _value;

            set
            {
                _value = value;

                OnValueChanged?.Invoke();
            }
        }

        public static implicit operator Room(RoomEvent @event)
            => @event.Value;
    }
}
