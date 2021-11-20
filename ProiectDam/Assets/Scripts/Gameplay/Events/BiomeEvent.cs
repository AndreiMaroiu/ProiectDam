using Gameplay.Generation;
using System;
using UnityEngine;

namespace Gameplay.Events
{
    [CreateAssetMenu(fileName = "New Biome Event", menuName = "Scriptables/Events/Biome Event")]
    public sealed class BiomeEvent : ScriptableObject
    {
        private BiomeType _value;

        public event Action OnValueChanged;

        public BiomeType Value
        {
            get => _value;

            set
            {
                _value = value;

                OnValueChanged?.Invoke();
            }
        }

        public static implicit operator BiomeType(BiomeEvent @event)
            => @event.Value;
    }
}
