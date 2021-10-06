using Random = System.Random;

public class WeightedRandom<T>
{
    private readonly Random _random = new Random();
    private readonly int[] _weights;
    private readonly T[] _elems;
    private readonly int _totalWeight;

    public WeightedRandom(T[] elems, int[] weigths)
    {
        _elems = elems;
        _weights = weigths;

        _totalWeight = 0;

        foreach (int weight in _weights)
        {
            _totalWeight += weight;
        }
    }

    public T RandomWeighted()
    {
        int sum = 0;
        int target = _random.Next(0, _totalWeight + 1);
        int resultIndex;

        for (resultIndex = 0; resultIndex < _weights.Length; resultIndex++)
        {
            sum += _weights[resultIndex];

            if (sum >= target)
            {
                break;
            }
        }

        return _elems[resultIndex];
    }
}
