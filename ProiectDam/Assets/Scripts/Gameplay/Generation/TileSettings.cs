using UnityEngine;
using Core;

namespace Gameplay.Generation
{
    [CreateAssetMenu(fileName = "New Tile Settings", menuName = "Scriptables/Settings/Tile Settings")]
    public class TileSettings : ScriptableObject
    {
        [SerializeField] private GameObject[] _none;
        [SerializeField] private GameObject[] _wall;
        [SerializeField] private GameObject[] _enemy;
        [SerializeField] private GameObject[] _door;
        [SerializeField] private GameObject[] _chest;
        [SerializeField] private GameObject[] _heal;
        [SerializeField] private GameObject[] _trap;
        [SerializeField] private GameObject[] _pickUp;
        [SerializeField] private GameObject[] _obstacle;
        [SerializeField] private GameObject[] _portal;

        public GameObject GetTile(TileType type)
        {
            GameObject[] _list = GetList(type);

            if (_list?.Length > 0)
            {
                return _list[Random.Range(0, _list.Length)];
            }

            return null;
        }

        private GameObject[] GetList(TileType type) => type switch
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
            _ => null,
        };
    }
}
