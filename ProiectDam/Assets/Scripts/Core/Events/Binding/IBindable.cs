using System;

namespace Core.Events.Binding
{
    public interface IBindable
    {
        Type ValueType { get; }
        bool Bind(IBindTarget target);
        bool UnBind(IBindTarget target);
    }

    public interface IBindable<T> : IBindable
    {
        event Action<T> OnValueChanged;
        bool Bind(Action<T> target);
        bool UnBind(Action<T> target);
    }
}
