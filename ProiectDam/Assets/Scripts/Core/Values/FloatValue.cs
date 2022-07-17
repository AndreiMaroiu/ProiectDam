using UnityEngine;

namespace Core.Values
{
    [CreateAssetMenu(fileName = "New Float Value", menuName = "Scriptables/Values/Float")]
    public class FloatValue : Value
    {
        [SerializeField] private float _value;

        public float Value => _value;

        public static implicit operator float(FloatValue value)
            => value.Value;

        public override string ToString()
            => Value.ToString();
    }
}
