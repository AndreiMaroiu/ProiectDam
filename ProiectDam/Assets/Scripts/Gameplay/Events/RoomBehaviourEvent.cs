using UnityEngine;
using System;
using Gameplay.Generation;

namespace Gameplay.Events
{
    public class RoomBehaviourEvent : ScriptableObject
    {
        public event Action OnValueChanged;

        private RoomBehaviour _value;

        public RoomBehaviour Value
        {
            get => _value;

            set
            {
                _value = value;

                OnValueChanged?.Invoke();
            }
        }

        public static implicit operator RoomBehaviour (RoomBehaviourEvent @event)
            => @event.Value;
    }
}
