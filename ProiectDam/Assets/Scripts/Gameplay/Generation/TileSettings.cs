using Core;
using System;
using UnityEngine;

namespace Gameplay.Generation
{
    [CreateAssetMenu(fileName = "New Tile Settings", menuName = "Scriptables/Settings/Tile Settings")]
    public class TileSettings : ScriptableObject
    {
        [SerializeField] private TileData[] _none;
        [SerializeField] private TileData[] _wall;
        [SerializeField] private TileData[] _enemy;
        [SerializeField] private TileData[] _door;
        [SerializeField] private TileData[] _chest;
        [SerializeField] private TileData[] _heal;
        [SerializeField] private TileData[] _trap;
        [SerializeField] private TileData[] _pickUp;
        [SerializeField] private TileData[] _obstacle;
        [SerializeField] private TileData[] _breakable;
        [SerializeField] private TileData[] _portal;
        [SerializeField] private TileData[] _merchant;

        private RoomType _currentRoomType;
        private Func<int, TileData, int> _tileChanceGetter;

        private void OnEnable()
        {
            _tileChanceGetter = GetTileChance; // get rid of heap allocation
        }

        public GameObject GetTile(TileType type, RoomType roomType)
        {
            _currentRoomType = roomType;
            TileData[] list = GetList(type);

            if (list != null && list.Length > 0)
            {
                WeightedRandom<TileData> random = new(list, _tileChanceGetter);
                return random.Take()?.Prefab;
            }

            return null;
        }

        public GameObject GetTileFromName(TileType type, string name)
        {
            TileData[] list = GetList(type);

            if (list == null)
            {
                return null;
            }

            foreach (var item in list)
            {
                if (item.Prefab.name == name)
                {
                    return item.Prefab;
                }
            }

            return null;
        }

        private TileData[] GetList(TileType type) => type switch
        {
            TileType.None => _none,
            TileType.Wall => _wall,
            TileType.Enemy => _enemy,
            TileType.Door => _door,
            TileType.Chest => _chest,
            TileType.Heal => _heal,
            TileType.Trap => _trap,
            TileType.PickUp => _pickUp,
            TileType.Obstacle => _obstacle,
            TileType.Portal => _portal,
            TileType.Merchant => _merchant,
            TileType.Breakable => _breakable,
            _ => null,
        };

        private int GetTileChance(int i, TileData elem)
        {
            if (elem.Flags.FastHasFlag(_currentRoomType))
            {
                return elem.SpawnChance;
            }

            return 0;
        }
    }
}
