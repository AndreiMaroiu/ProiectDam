using Core.Events.Binding;
using UnityEngine;

namespace Core
{
    public abstract class Signal<T> : ScriptableObject, IBindSource<T>
    {
        private readonly BindableValue<T> _value = new();

        public IBindable<T> Bindable => _value;
        IBindable IBindSource.SimpleBindable => _value;

        protected abstract T CalculateValue();

        protected void Bind(IBindable<T> bindable)
        {
            bindable.OnValueChanged += _ => OnValueChange();
        }

        protected abstract void OnEnable();

        private void OnValueChange()
        {
            _value.Value = CalculateValue();
        }
    }
}
