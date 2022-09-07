using Core.Events;
using Gameplay.Enemies;
using Gameplay.Events;
using Gameplay.Generation;
using Gameplay.Player;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
using Gameplay.DataSaving;

namespace Gameplay.Managers
{
    public class RoomManager : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Gizmos")]
        [SerializeField] private bool _drawGizmos;
        [SerializeField] private bool _fillSquare;
#endif
        [Header("Game objects")]
        [SerializeField] private BaseLevelSpawner _spawner;
        [SerializeField] private PlayerController _player;
        [Tooltip("Leave empty if no level saver is needed")]
        [SerializeField] private RandomLevelSaverManager _levelSaver;
        [Header("Events")]
        [SerializeField] private RoomEvent _roomEvent;
        [SerializeField] private RoomBehaviourEvent _roomBehaviourEvent;
        [SerializeField] private IntEvent _layersNumberEvent;
        [SerializeField] private IntEvent _currentLayerEvent;
        [SerializeField] private BiomeEvent _biomeEvent;
        [SerializeField] private BoolEvent _previewEvent;
        [SerializeField] private BoolEvent _playerTurn;

        private int _lastLayer;
        private bool _canChangeLayer = true;

        public void Awake()
        {
            InitEvents();

            _spawner.Spawn();
            _roomEvent.StartRoom = _spawner.Traverser.Start.Room;
            RoomBehaviour behaviour = GetActiveBehaviour();

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
            _playerTurn.Value = true;
        }

        private void OnDestroy()
        {
            _roomBehaviourEvent.OnValueChanged -= OnRoomChanged;
            _currentLayerEvent.OnValueChanged -= OnLayerChanged;
            _previewEvent.OnValueChanged -= OnPreviewChanged;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (Application.isPlaying && _drawGizmos)
            {
                int size = _roomBehaviourEvent.Value.ActiveLayer.GetLength(0);
                TileType[,] layer = _roomBehaviourEvent.Value.ActiveLayer;

                Vector3 offset = Utils.GetVector3FromMatrixPos(size / 2, size / 2, 1.3f)
                                        - _roomBehaviourEvent.Value.transform.position;

                for (int row = 0; row < size; row++)
                {
                    for (int column = 0; column < size; column++)
                    {
                        Vector3 where = Utils.GetVector3FromMatrixPos(row, column, 1.3f) - offset;
                        Gizmos.color = GetGizmoColor(layer[row, column]);
                        if (_fillSquare)
                        {
                            Gizmos.DrawCube(where, Vector3.one);
                        }
                        else
                        {
                            Gizmos.DrawWireCube(where, Vector3.one);
                        }
                        Handles.Label(where, $"({row.ToString()}, {column.ToString()})");
                    }
                }
            }
        }

#endif

        private Color GetGizmoColor(TileType tile) => tile switch
        {
            TileType.None => Color.white,
            TileType.Wall => new Color(0.42f, 0.20f, 0.20f), // brown
            TileType.Enemy => Color.red,
            TileType.Door => new Color(0.7f, 0.7f, 0.7f), //dark gray
            TileType.Chest => new Color(255, 165, 0), // orange
            TileType.Heal => Color.green,
            TileType.Trap => Color.magenta,
            TileType.PickUp => Color.blue,
            TileType.Obstacle => Color.yellow,
            TileType.Portal => Color.black,
            TileType.Player => Color.cyan,
            TileType.Merchant => new Color(11, 83, 69), // dark cyan
            TileType.Breakable => new Color(121, 85, 72), // light brown
            _ => Color.clear,
        };

        private RoomBehaviour GetActiveBehaviour()
        {
            if (_levelSaver.ShouldLoad)
            {
                RoomBehaviour roomBehaviour = _spawner.Traverser[_levelSaver.SaveData.CurrentRoom];
                _roomBehaviourEvent.Value = roomBehaviour;
                return roomBehaviour;
            }

            return _roomBehaviourEvent.Value;
        }

        private void SetPlayerPos(RoomBehaviour behaviour)
        {
            if (_levelSaver != null && _levelSaver.ShouldLoad)
            {
                int layerIndex = _levelSaver.SaveData.PlayerData.LayerPos.Biome;
                TileType[,] layer = behaviour.Layers[layerIndex];
                var pos = _levelSaver.SaveData.PlayerData.LayerPos.Position;
                layer[pos.X, pos.Y] = TileType.Player; // todo: this may not be needed in future
                _player.LayerPosition = new LayerPosition(pos, layer);
                _player.transform.position = _levelSaver.SaveData.PlayerData.PlayerPos;

                _currentLayerEvent.Value = layerIndex;
                behaviour.ChangedLayer(layerIndex);
                _biomeEvent.Value = _roomBehaviourEvent.Value.Layers.GetBiome(layerIndex);
            }
            else
            {
                TileType[,] layer = behaviour.Layers[behaviour.CurrentLayer];
                int middle = layer.GetLength(0) / 2;
                layer[middle, middle] = TileType.Player;
                _player.LayerPosition = new LayerPosition(new Vector2Int(middle, middle), layer);
            }
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
                VibrationManager.Instance.TryVibrate();
            }
        }

        private void OnPreviewChanged()
        {
            if (_previewEvent)
            {
                _lastLayer = _currentLayerEvent.Value;
                return;
            }

            // exits preview
            if (!_canChangeLayer)
            {
                _currentLayerEvent.Value = _lastLayer;
            }

            _player.LayerPosition.Layer = _roomBehaviourEvent.Value.Layers[_currentLayerEvent.Value];
            _player.MakePlayerWhite();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(Scenes.MainScene);
            }
        }
#endif
    }
}
