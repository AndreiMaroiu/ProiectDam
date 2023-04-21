using System.Diagnostics;

namespace Core
{
    [DebuggerDisplay("{Value} {HasValue}")]
    public struct Optional<T> where T : UnityEngine.Object
    {
        private T _value;
        private bool _hasValue;

        public Optional(T value)
        {
            UnityEngine.Debug.Assert(value != null);

            _value = value;
            _hasValue = true;
        }

        public T Value
        {
            readonly get => _value;
            set
            {
                _value = value;
                _hasValue = value is not null;
            }
        }

        public readonly bool HasValue => _hasValue;
    }
}
