using UnityEngine;

namespace ObjectPooling
{
    [CreateAssetMenu(fileName = "New Pool Data", menuName = "Scriptables/ObjectPools/Pool Data")]
    public class PoolData : ScriptableObject
    {
        [SerializeField] private int _poolSize;
        [SerializeField] private PoolType _type;
        [SerializeField] private GameObject[] _prefabs;

        private IObjectPool _pool;

        public IObjectPool Pool
        {
            get
            {
                if (_pool is null)
                {
                    _pool = GeneratePool();
                }

                return _pool;
            }
        }

        private IObjectPool GeneratePool()
        {
            if (_prefabs.Length == 0)
            {
                return null;
            }

            return _type switch
            {
                PoolType.Simple => new SimplePool(_poolSize, _prefabs[0]),
                PoolType.Random => new RandomPool(_prefabs, _poolSize),
                _ => null,
            };
        }
    }
}
