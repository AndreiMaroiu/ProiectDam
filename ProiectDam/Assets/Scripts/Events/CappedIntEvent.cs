using System;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "New Capped Int Event", menuName = "Scriptables/Events/Capped Int Event")]
    public class CappedIntEvent : ScriptableObject
    {
        public event Action OnHealthChanged;
        public event Action OnMaxHealthChanged;

        private int _value;
        private int _maxValue;

        public int Value
        {
            get => _value;

            set
            {
                _value = value;

                if (_value > _maxValue)
                {
                    _value = _maxValue;
                }

                OnHealthChanged?.Invoke();
            }
        }

        public int MaxValue
        {
            get => _maxValue;

            set
            {
                _maxValue = value;
                OnMaxHealthChanged?.Invoke();
            }
        }
    }
}
