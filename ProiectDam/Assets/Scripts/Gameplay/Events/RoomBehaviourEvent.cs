using UnityEngine;
using System;
using Gameplay.Generation;

namespace Gameplay.Events
{
    [CreateAssetMenu(fileName = "New Room Behaviour Event", menuName = "Scriptables/Events/Room Behaviour Event")]
    public class RoomBehaviourEvent : ScriptableObject
    {
        public event Action OnValueChanged;

        private RoomBehaviour _value;

        public RoomBehaviour Value
        {
            get => _value;

            set
            {
                if (_value == value)
                {
                    return;
                }

                _value = value;

                OnValueChanged?.Invoke();
            }
        }

        public static implicit operator RoomBehaviour (RoomBehaviourEvent @event)
            => @event.Value;
    }
}
