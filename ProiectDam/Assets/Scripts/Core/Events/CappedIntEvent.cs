using System;
using UnityEngine;
using Core.Events.Binding;

namespace Core.Events
{
    /// <summary>
    /// Can be used to represent int values with a maximum value
    /// </summary>
    [CreateAssetMenu(fileName = "New Capped Int Event", menuName = "Scriptables/Events/Capped Int Event")]
    public sealed class CappedIntEvent : ScriptableObject
    {
        private readonly BindableValue<int> _valueEvent = new();
        private readonly BindableValue<int> _maxValueEvent = new();

        public event Action<int> OnValueChanged
        {
            add => _valueEvent.OnValueChanged += value;
            remove => _valueEvent.OnValueChanged -= value;
        }
        public event Action<int> OnMaxValueChanged
        {
            add => _maxValueEvent.OnValueChanged += value;
            remove => _maxValueEvent.OnValueChanged -= value;
        }

        public int Value
        {
            get => _valueEvent.Value;

            set
            {
                if (value > _maxValueEvent.Value)
                {
                    value = _maxValueEvent.Value;
                }

                _valueEvent.Value = value;
            }
        }

        public int MaxValue
        {
            get => _maxValueEvent.Value;
            set => _maxValueEvent.Value = value;
        }

        public IBindable<int> ValueBindable => _valueEvent;
        public IBindable<int> MaxValueBindable => _maxValueEvent;

        public void Init(int value)
        {
            MaxValue = value;
            Value = value;
        }

        public void Set(int current, int max)
        {
            MaxValue = max;
            Value = current;
        }

        public override string ToString()
            => $"{Value.ToString()}/{MaxValue.ToString()}";
    }
}
