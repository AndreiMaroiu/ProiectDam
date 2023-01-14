using Core;
using UnityEngine;

namespace Gameplay.Generation
{
    [System.Serializable]
    public class TileData
    {
        public const int DefaultFlags = ~0;
        public const int DefaultSpawnChance = 1;

        [SerializeField] private GameObject _prefab;
        [SerializeField] private RoomType _flags = (RoomType)DefaultFlags;
        [SerializeField] private int _spawnChance = DefaultSpawnChance;
        [SerializeField] private bool _onlyOnce;

        private bool _spawned;

        public GameObject Prefab => _prefab;
        public RoomType Flags => _flags;
        public int SpawnChance => (_onlyOnce && _spawned) ? 0 : _spawnChance;

        public bool OnlyOnce => _onlyOnce;

        public void SetAsSpawned() => _spawned = true;

        public void Reset() => _spawned = false;
    }
}
