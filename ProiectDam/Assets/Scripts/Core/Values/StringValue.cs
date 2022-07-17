using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Values
{
    [CreateAssetMenu(fileName = "NewStringValue", menuName = "Scriptables/Values/String Value")]
    public class StringValue : Value
    {
        [SerializeField, TextArea] private string _value;

        public string Value => _value;

        public static implicit operator string (StringValue value)
            => value.Value;

        public override string ToString()
            => Value;
    }
}
