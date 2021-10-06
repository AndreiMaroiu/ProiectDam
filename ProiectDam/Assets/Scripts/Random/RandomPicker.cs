using System;
using Random = System.Random;

public class RandomPicker<T>
{
    private readonly Random _random = new Random();
    private readonly T[] _elems;
    private int _max;

    public RandomPicker(T[] elems)
    {
        _elems = new T[elems.Length];
        Array.Copy(elems, _elems, elems.Length);
        Reset();
    }

    public int MaxSize => _elems.Length;

    public void Reset() => _max = _elems.Length;

    public bool CanTake => _max >= 0;

    public T Take()
    {
        int index = _random.Next(0, _max);
        --_max;
        Swap(index);

        return _elems[_max];
    }

    private void Swap(int index)
    {
        T copy = _elems[index];
        _elems[index] = _elems[_max];
        _elems[_max] = copy;
    }
}
