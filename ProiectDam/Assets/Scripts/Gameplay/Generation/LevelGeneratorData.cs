using Core.Values;
using Gameplay.Events;
using Gameplay.Generation;
using UnityEngine;

namespace Gameplay.Generation
{
    [CreateAssetMenu(fileName = "New Level Generator Data", menuName = "Scriptables/Settings/Level Generator Data")]
    public sealed class LevelGeneratorData : ScriptableObject
    {
        [SerializeField] private float _distance;
        [Header("Prefabs")]
        [SerializeField] private RoomBehaviour _room;
        [SerializeField] private DoorBehaviour _door;
        [Header("Tiles")]
        [SerializeField] private TileSettings _dungeonTiles;
        [SerializeField] private TileSettings _fireTiles;
        [SerializeField] private TileSettings _grassTiles;
        [Header("Settings")]
        [SerializeField] private FloatValue _cellSize;
        [SerializeField] private int _cellCount;
        [Header("Events")]
        [SerializeField] private RoomBehaviourEvent _roomBehaviourEvent;

        public float Distance => _distance;
        public RoomBehaviour Room => _room;
        public DoorBehaviour Door => _door;
        public TileSettings DungeonTiles => _dungeonTiles;
        public TileSettings FireTiles => _fireTiles;
        public TileSettings GrassTiles => _grassTiles;
        public float CellSize => _cellSize;
        public int CellCount => _cellCount;
        public RoomBehaviourEvent RoomBehaviourEvent => _roomBehaviourEvent;
    }
}