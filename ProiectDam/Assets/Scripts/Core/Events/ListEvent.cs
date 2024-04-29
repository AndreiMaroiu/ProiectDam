using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Events
{
    public abstract class ListEvent<T> : SceneAwareSO, IEnumerable<T>
    {
        private readonly List<T> _list = new();

        protected override void OnDisable()
        {
            _list.Clear();
        }

        protected override void OnEnable()
        {

        }

        public event Action<T> OnElementAdded;
        public event Action<T> OnElementRemoved;

        public ReadonlyList<T> Values => new(_list);

        public void Add(T elem)
        {
            _list.Add(elem);
            OnElementAdded?.Invoke(elem);
        }

        public void Remove(T elem)
        {
            _list.Remove(elem);
            OnElementRemoved?.Invoke(elem);
        }

        public List<T>.Enumerator GetEnumerator() => _list.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
    }

    public readonly struct ReadonlyList<T> : IEnumerable<T>
    {
        private readonly List<T> _list;

        internal ReadonlyList(List<T> list)
        {
            _list = list;
        }

        public T this[int index] => _list[index];

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

        public List<T>.Enumerator GetEnumerator() => _list.GetEnumerator();
    }
}
