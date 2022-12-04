using System;
using UnityEngine;
using Core.Events.Binding;

namespace Core.Events
{
    /// <summary>
    /// Can be used to represent int values with a maximum value
    /// </summary>
    [CreateAssetMenu(fileName = "New Capped Int Event", menuName = "Scriptables/Events/Capped Int Event")]
    public sealed class CappedIntEvent : ScriptableObject, IBindSource<(int value, int max)>
    {
        [SerializeField] private BindableValue<int> _valueEvent = new();
        [SerializeField] private BindableValue<int> _maxValueEvent = new();
        private readonly BindableEvent<(int value, int max)> _allEvent = new();

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

        public event Action<(int value, int max)> OnAllValuesChanged
        {
            add => _allEvent.OnValueChanged += value;
            remove => _allEvent.OnValueChanged -= value;
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

        public IBindable<(int value, int max)> Bindable => _allEvent;

        public IBindable SimpleBindable => _allEvent;

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

        private void OnEnable()
        {
            _valueEvent.OnValueChanged += TriggerAll;
            _maxValueEvent.OnValueChanged += TriggerAll;
        }

        private void TriggerAll(int _)
        {
            _allEvent.Invoke((Value, MaxValue));
        }

        private void OnDisable()
        {
            _valueEvent.OnValueChanged -= TriggerAll;
            _maxValueEvent.OnValueChanged -= TriggerAll;
        }

        public override string ToString()
            => $"{Value.ToString()}/{MaxValue.ToString()}";
    }
}
