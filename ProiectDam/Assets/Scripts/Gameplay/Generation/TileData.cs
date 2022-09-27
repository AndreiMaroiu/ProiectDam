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

        public GameObject Prefab => _prefab;
        public RoomType Flags => _flags;
        public int SpawnChance => _spawnChance;

        public void SetPrefab(GameObject prefab) => _prefab = prefab;
    }
}
