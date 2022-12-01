using System;
using Random = UnityEngine.Random;

public struct WeightedRandom<T>
{
    private readonly int[] _weights;
    private readonly T[] _elems;
    private readonly int _totalWeight;
    private readonly Func<int, T, int> _weightGetter;

    public WeightedRandom(T[] elems, int[] weigths)
    {
        _elems = elems;
        _weights = weigths;
        _totalWeight = 0;
        _weightGetter = null;

        foreach (int weight in _weights)
        {
            _totalWeight += weight;
        }

        _weightGetter = SimpleWeightGetter;
    }

    public WeightedRandom(T[] elems, Func<int, T, int> weightGetter)
    {
        _elems = elems;
        _weights = null;
        _totalWeight = 0;
        _weightGetter = weightGetter;

        for (int i = 0; i < elems.Length; i++)
        {
            _totalWeight += weightGetter(i, elems[i]);
        }
    }

    private int SimpleWeightGetter(int i, T elem)
    {
        return _weights[i];
    }

    public T Take()
    {
#if UNITY_EDITOR || DEBUG
        if (_totalWeight == 0)
        {
            throw new ZeroTotalWeightException("Total weight was zero!");
        }
#endif

        int sum = 0;
        int target = Random.Range(0, _totalWeight + 1);
        int resultIndex;

        for (resultIndex = 0; resultIndex < _elems.Length; resultIndex++)
        {
            int weight = _weightGetter(resultIndex, _elems[resultIndex]);

            if (weight is 0)
            {
                continue;
            }

            sum += weight;

            if (sum >= target)
            {
                break;
            }
        }

        return _elems[resultIndex];
    }
}


[Serializable]
public class ZeroTotalWeightException : Exception
{
    public ZeroTotalWeightException() { }
    public ZeroTotalWeightException(string message) : base(message) { }
    public ZeroTotalWeightException(string message, Exception inner) : base(message, inner) { }
    protected ZeroTotalWeightException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
