using System;
using Random = UnityEngine.Random;

public struct RandomPicker<T>
{
    private readonly T[] _elems;
    private int _max;

    public RandomPicker(T[] elems, bool createCopy = true)
    {
        if (createCopy)
        {
            _elems = new T[elems.Length];
            Array.Copy(elems, _elems, elems.Length);
        }
        else
        {
            _elems = elems;
        }

        _max = _elems.Length;
    }

    public readonly int MaxSize => _elems.Length;

    public void Reset() => _max = _elems.Length;

    public readonly bool CanTake => _max > 0;

    public T Take()
    {
        int index = Random.Range(0, _max);
        --_max;
        Swap(index);

        return _elems[_max];
    }

    private readonly void Swap(int index)
    {
        T copy = _elems[index];
        _elems[index] = _elems[_max];
        _elems[_max] = copy;
    }
}
