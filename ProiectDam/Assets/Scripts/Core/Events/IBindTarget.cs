namespace Core.Events
{
    public interface IBindTarget
    {

    }

    public interface IBindTarget<T> : IBindTarget
    {
        public void OnValueChange(T newValue);
    }
}
