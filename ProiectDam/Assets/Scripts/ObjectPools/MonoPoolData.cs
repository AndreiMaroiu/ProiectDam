using UnityEngine;

namespace ObjectPooling
{
    [CreateAssetMenu(fileName = "New Mono Pool Data", menuName = "Scriptables/ObjectPools/Mono Pool Data")]
    public class MonoPoolData : ScriptableObject
    {
        [SerializeField] private int _poolSize;
        [SerializeField] private PoolType _type;
        [SerializeField] private MonoBehaviour[] _prefabs;

        private IObjectPool<MonoBehaviour> _pool;

        public IObjectPool<MonoBehaviour> Pool
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

        private IObjectPool<MonoBehaviour> GeneratePool()
        {
            if (_prefabs.Length == 0)
            {
                return null;
            }

            return _type switch
            {
                PoolType.Simple => new SimplePool<MonoBehaviour>(_poolSize, _prefabs[0]),
                PoolType.Random => new RandomPool<MonoBehaviour>(_prefabs, _poolSize),
                _ => null,
            };
        }
    }
}
