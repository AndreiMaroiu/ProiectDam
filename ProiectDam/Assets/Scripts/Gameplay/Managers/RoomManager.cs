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
        [SerializeField] private BoolEvent _previewEvent;

        private int _lastLayer;
        private bool _canChangeLayer = true;

        public void Awake()
        {
            InitEvents();

            _spawner.Spawn();
            RoomBehaviour behaviour = _roomBehaviourEvent.Value;

            SetPlayerPos(behaviour);

            _roomBehaviourEvent.OnValueChanged += OnRoomChanged;
            _currentLayerEvent.OnValueChanged += OnLayerChanged;
            _previewEvent.OnValueChanged += OnPreviewChanged;

            _currentLayerEvent.Value = behaviour.CurrentLayer;
            _layersNumberEvent.Value = behaviour.Layers.Count;
            _roomEvent.Value = behaviour.Room;
        }

        private void InitEvents()
        {
            _previewEvent.Value = false;
        }

        private void OnDestroy()
        {
            _roomBehaviourEvent.OnValueChanged -= OnRoomChanged;
            _currentLayerEvent.OnValueChanged -= OnLayerChanged;
            _previewEvent.OnValueChanged -= OnPreviewChanged;
        }

        private void SetPlayerPos(RoomBehaviour behaviour)
        {
            TileType[,] layer = behaviour.Layers[behaviour.CurrentLayer];
            int middle = layer.GetLength(0) / 2;
            _player.LayerPosition = new LayerPosition(new Vector2Int(middle, middle), layer);
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

            ChangePlayerColor();

            _biomeEvent.Value = _roomBehaviourEvent.Value.Layers.GetBiome(layer);
        }

        private void ChangePlayerColor()
        {
            if (!_previewEvent.Value)
            {
                return;
            }

            if (_lastLayer == _currentLayerEvent.Value)
            {
                _player.MakePlayerTransparent();
                _canChangeLayer = true;
                return;
            }

            RoomBehaviour behaviour = _roomBehaviourEvent.Value;
            Vector2Int playerPos = _player.LayerPosition.Position;
            TileType[,] previewLayer = behaviour.Layers.GetTiles(_currentLayerEvent.Value);

            if (previewLayer[playerPos.x, playerPos.y] is TileType.None)
            {
                _player.MakePlayerGreen();
                _canChangeLayer = true;
            }
            else
            {
                _player.MakePlayerRed();
                _canChangeLayer = false;
            }
        }

        private void OnPreviewChanged()
        {
            if (_previewEvent.Value)
            {
                _lastLayer = _currentLayerEvent.Value;
            }
            else
            {
                if (!_canChangeLayer)
                {
                    _currentLayerEvent.Value = _lastLayer;
                }

                _player.LayerPosition.Layer = _roomBehaviourEvent.Value.Layers[_currentLayerEvent.Value];
                _player.MakePlayerWhite();
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }
#endif
    }
}
