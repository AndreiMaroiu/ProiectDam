using UnityEngine;

namespace Core.Values
{
    public abstract class BaseValue<T> : ScriptableObject
    {
        [SerializeField] private T _value;

        public T Value
        {
            get => _value;
            set => _value = value;
        }

        public static implicit operator T (BaseValue<T> value)
            => value._value;

        public override string ToString()
            => _value.ToString();
    }
}
