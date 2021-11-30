using UnityEngine;

namespace Values
{
    [CreateAssetMenu(fileName = "NewIntValue", menuName = "Scriptables/Values/Int Value")]
    public class IntValue : Value
    {
        [SerializeField] private int _value;

        public int Value => _value;

        public static implicit operator int(IntValue value)
            => value.Value;

        public override string ToString()
            => Value.ToString();
    }
}
