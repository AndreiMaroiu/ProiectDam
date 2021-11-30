using System;
using UnityEngine;

namespace Events
{
    /// <summary>
    /// Can be used to represent int values with a maximum value
    /// </summary>
    [CreateAssetMenu(fileName = "New Capped Int Event", menuName = "Scriptables/Events/Capped Int Event")]
    public class CappedIntEvent : ScriptableObject
    {
        public event Action OnValueChanged;
        public event Action OnMaxValueChanged;

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

                OnValueChanged?.Invoke();
            }
        }

        public void Init(int value)
        {
            MaxValue = value;
            Value = value;
        }

        public int MaxValue
        {
            get => _maxValue;

            set
            {
                _maxValue = value;
                OnMaxValueChanged?.Invoke();
            }
        }

        public override string ToString()
            => $"{Value.ToString()}/{MaxValue.ToString()}";
    }
}
