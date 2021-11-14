using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Values
{
    [CreateAssetMenu(fileName = "New Float Value", menuName = "Scriptables/Values/Float")]
    public class FloatValue : ScriptableObject
    {
        [SerializeField] private float _value;

        public float Value => _value;

        public static implicit operator float (FloatValue value)
            => value.Value;
    }
}
