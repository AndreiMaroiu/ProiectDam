using System;

namespace Core.Events.Binding
{
    public class BindableEvent<T> : IBindable<T>
    {
        public Type ValueType => typeof(T);

        public event Action<T> OnValueChanged;

        public void Invoke(T value) => OnValueChanged?.Invoke(value);

        public bool Bind(IBindTarget target)
        {
            if (target is IBindTarget<T> t)
            {
                OnValueChanged += t.OnValueChange;
                return true;
            }

            return false;
        }

        public bool UnBind(IBindTarget target)
        {
            if (target is IBindTarget<T> t)
            {
                OnValueChanged -= t.OnValueChange;
                return true;
            }

            return false;
        }
    }
}
