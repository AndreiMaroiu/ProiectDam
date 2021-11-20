using Events;
using Gameplay.Events;
using Gameplay.Generation;
using Gameplay.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay.Managers
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private LevelSpawner _spawner;
        [SerializeField] private PlayerController _player;
        [Header("Events")]
        [SerializeField] private RoomEvent _roomEvent;
        [SerializeField] private RoomBehaviourEvent _roomBehaviourEvent;
        [SerializeField] private IntEvent _layersNumberEvent;
        [SerializeField] private IntEvent _currentLayerEvent;
        [SerializeField] private BiomeEvent _biomeEvent;

        public void Awake()
        {
            _spawner.Spawn();
            RoomBehaviour behaviour = _roomBehaviourEvent.Value;

            TileType[,] layer = behaviour.Layers[behaviour.CurrentLayer];
            int middle = layer.GetLength(0) / 2;
            _player.LayerPosition = new LayerPosition(new Vector2Int(middle, middle), layer);

            _roomBehaviourEvent.OnValueChanged += OnRoomChanged;
            _currentLayerEvent.OnValueChanged += OnLayerChanged;

            _currentLayerEvent.Value = behaviour.CurrentLayer;
            _layersNumberEvent.Value = behaviour.Layers.Count;
        }

        private void OnRoomChanged()
        {
            RoomBehaviour room = _roomBehaviourEvent;

            _roomEvent.Value = room.Room;
            _currentLayerEvent.Value = room.CurrentLayer;
            _layersNumberEvent.Value = room.Layers.Count;
        }

        private void OnLayerChanged()
        {
            int layer = _currentLayerEvent.Value;

            _roomBehaviourEvent.Value.ChangedLayer(layer);
            _player.LayerPosition.Layer = _roomBehaviourEvent.Value.Layers[layer];

            _biomeEvent.Value = _roomBehaviourEvent.Value.Layers.GetBiome(layer);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
