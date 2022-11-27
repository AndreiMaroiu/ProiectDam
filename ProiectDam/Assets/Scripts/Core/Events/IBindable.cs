using System;

namespace Core.Events
{
    public interface IBindable
    {
        Type ValueType { get; }
        bool Bind(IBindTarget target);
        bool UnBind(IBindTarget target);
    }

    public interface IBindable<T> : IBindable
    {
        T Value { get; set; }
        event Action<T> OnValueChanged;
    }
}
