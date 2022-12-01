namespace Core.Events.Binding
{
    public interface IBindSource
    {
        IBindable SimpleBindable { get; }
    }

    public interface IBindSource<T> : IBindSource
    {
        IBindable<T> Bindable { get; }
    }
}
