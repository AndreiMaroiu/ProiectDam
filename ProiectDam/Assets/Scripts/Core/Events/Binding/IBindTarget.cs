namespace Core.Events.Binding
{
    public interface IBindTarget
    {

    }

    public interface IBindTarget<T> : IBindTarget
    {
        public void OnValueChange(T newValue);
    }
}
