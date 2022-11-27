using Core.Events;
using Gameplay.Events;
using Gameplay.Generation;
using Gameplay.Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

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
        [SerializeField] protected BaseLevelSpawner _spawner;
        [SerializeField] protected PlayerController _player;
        [Header("Events")]
        [SerializeField] protected RoomEvent _roomEvent;
        [SerializeField] protected RoomBehaviourEvent _roomBehaviourEvent;
        [SerializeField] protected LayerEvent _layerEvent;
        [SerializeField] protected BoolEvent _previewEvent;
        [SerializeField] protected BoolEvent _playerTurn;

        private int _lastLayer;

        public bool IsSameLayer { get; protected set; }
        public bool CanSwitchLayer { get; protected set; } = true;

        #region Unity Events

        private void Awake()
        {
            InitEvents();

            _spawner.Spawn();
            _roomEvent.StartRoom = _spawner.Traverser.Start.Room;
            RoomBehaviour behaviour = GetActiveBehaviour();

            SetPlayerPos(behaviour);

            _roomBehaviourEvent.OnValueChanged += OnRoomChanged;
            _layerEvent.CurrentLayer.OnValueChanged += OnLayerChanged;
            _previewEvent.OnValueChanged += OnPreviewChanged;

            _layerEvent.CurrentLayer.Value = behaviour.CurrentLayer;
            _layerEvent.LayerCount.Value = behaviour.Layers.Count;
            _roomEvent.Value = behaviour.Room;
            behaviour.Room.Discovered = true;
        }

        private void OnDestroy()
        {
            _roomBehaviourEvent.OnValueChanged -= OnRoomChanged;
            _layerEvent.CurrentLayer.OnValueChanged -= OnLayerChanged;
            _previewEvent.OnValueChanged -= OnPreviewChanged;
        }

        #endregion

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
            TileType.DynamicObstacle => new Color(121, 85, 72), // light brown
            _ => Color.clear,
        };

#endif

        private void InitEvents()
        {
            _previewEvent.Value = false;
            _playerTurn.Value = true;
        }

        protected virtual RoomBehaviour GetActiveBehaviour()
        {
            return _roomBehaviourEvent.Value;
        }

        protected virtual void SetPlayerPos(RoomBehaviour behaviour)
        {
            TileType[,] layer = behaviour.Layers[behaviour.CurrentLayer];
            int middle = layer.GetLength(0) / 2;
            layer[middle, middle] = TileType.Player;
            _player.LayerPosition = new(new Vector2Int(middle, middle), layer);
        }

        private void OnRoomChanged()
        {
            RoomBehaviour room = _roomBehaviourEvent;

            _roomEvent.Value = room.Room;
            _layerEvent.CurrentLayer.Value = room.CurrentLayer;
            _layerEvent.LayerCount.Value = room.Layers.Count;

            room.Room.Discovered = true;
        }

        private void OnLayerChanged(int layer = 0)
        {
            _roomBehaviourEvent.Value.ChangedLayer(layer);
            _layerEvent.CurrentBiome.Value = _roomBehaviourEvent.Value.Layers.GetBiome(layer);

            ChangePlayerColor();
        }

        private void ChangePlayerColor()
        {
            if (!_previewEvent.Value)
            {
                return;
            }

            if (_lastLayer == _layerEvent.CurrentLayer)
            {
                _player.MakePlayerTransparent();
                CanSwitchLayer = true;
                IsSameLayer = true;
                return;
            }

            RoomBehaviour behaviour = _roomBehaviourEvent.Value;
            Vector2Int playerPos = _player.LayerPosition.Position;
            TileType[,] previewLayer = behaviour.Layers.GetTiles(_layerEvent.CurrentLayer);
            IsSameLayer = false;

            if (previewLayer[playerPos.x, playerPos.y] is TileType.None)
            {
                _player.MakePlayerGreen();
                CanSwitchLayer = true;
            }
            else
            {
                _player.MakePlayerRed();
                CanSwitchLayer = false;
                VibrationManager.Instance.TryVibrate();
            }
        }

        private void OnPreviewChanged(bool preview)
        {
            if (preview)
            {
                _lastLayer = _layerEvent.CurrentLayer;
                return;
            }

            // exits preview
            if (!CanSwitchLayer)
            {
                _layerEvent.CurrentLayer.Value = _lastLayer;
            }

            _player.LayerPosition.Layer = _roomBehaviourEvent.Value.Layers[_layerEvent.CurrentLayer];
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
