using Core;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Generation
{
    [Serializable]
    public class TileData
    {
        public const int DefaultFlags = ~0;
        public const int DefaultSpawnChance = 1;

        [SerializeField] private AssetReferenceGameObject _prefab;
        [SerializeField] private RoomType _flags = (RoomType)DefaultFlags;
        [SerializeField] private int _spawnChance = DefaultSpawnChance;
        [SerializeField] private bool _onlyOnce;

        private Optional<GameObject> _gameObject = new();
        private bool _spawned;
        private Guid? _loadedGuid = null;

        public GameObject Prefab => GetOrLoadPrefab();
        public RoomType Flags => _flags;
        public int SpawnChance => (_onlyOnce && _spawned) ? 0 : _spawnChance;

        public bool OnlyOnce => _onlyOnce;

        public void SetAsSpawned() => _spawned = true;

        public void Reset() => _spawned = false;
        public Guid TileGuid => _loadedGuid ??= Guid.Parse(_prefab.AssetGUID);

        private GameObject GetOrLoadPrefab()
        {
            if (_gameObject.HasValue)
            {
                return _gameObject.Value;
            }

            if (_prefab is null)
            {
                return null;
            }

            var handle = _prefab.LoadAssetAsync<GameObject>();
            _gameObject.Value = handle.WaitForCompletion();

            return _gameObject.Value;
        }
    }
}
