using Events;
using Gameplay.Enemies;
using Gameplay.Events;
using Gameplay.Generation;
using Gameplay.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Gameplay.Managers
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private bool _drawGizmos;
        [SerializeField] private LevelSpawner _spawner;
        [SerializeField] private PlayerController _player;
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
            RoomBehaviour behaviour = _roomBehaviourEvent.Value;

            SetPlayerPos(behaviour);

            _roomBehaviourEvent.OnValueChanged += OnRoomChanged;
            _currentLayerEvent.OnValueChanged += OnLayerChanged;
            _previewEvent.OnValueChanged += OnPreviewChanged;
            _playerTurn.OnValueChanged += OnPlayerMoveEnd;

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
            _playerTurn.OnValueChanged -= OnPlayerMoveEnd;
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying && _drawGizmos)
            {
                int size = _roomBehaviourEvent.Value.ActiveLayer.GetLength(0);
                TileType[,] layer = _roomBehaviourEvent.Value.ActiveLayer;

                Vector3 offset = Utils.GetVector3FromMatrixPos(size / 2, size / 2, 1.3f) 
                                        - _roomBehaviourEvent.Value.transform.position;

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        Vector3 where = Utils.GetVector3FromMatrixPos(i, j, 1.3f) - offset;
                        Gizmos.color = GetGizmoColor(layer[i, j]);
                        Gizmos.DrawCube(where, Vector3.one);
                    }
                }
            }
        }

        private Color GetGizmoColor(TileType tile) => tile switch
        {
            TileType.None => Color.white,
            TileType.Wall => new Color(0.42f, 0.20f, 0.20f),// brown
            TileType.Enemy => Color.red,
            TileType.Door => Color.gray,
            TileType.Chest => new Color(255, 165, 0),// orange
            TileType.Heal => Color.green,
            TileType.Trap => Color.magenta,
            TileType.PickUp => Color.blue,
            TileType.Obstacle => Color.yellow,
            TileType.Portal => Color.black,
            TileType.Player => Color.cyan,
            _ => Color.clear,
        };

        private void SetPlayerPos(RoomBehaviour behaviour)
        {
            TileType[,] layer = behaviour.Layers[behaviour.CurrentLayer];
            int middle = layer.GetLength(0) / 2;
            layer[middle, middle] = TileType.Player;
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

        private IEnumerator ProcessEnemies()
        {
            foreach (BaseEnemy enemy in _roomBehaviourEvent.Value.ActiveLayerBehaviour.Enemies)
            {
                enemy.OnEnemyTurn(_player);
                
                yield return new WaitForSeconds(enemy.MoveTime + 0.1f);
                Debug.Log("Enemy finished!");
            }

            _playerTurn.Value = true;
        }

        private void OnPlayerMoveEnd()
        {
            if (!_playerTurn)
            {
                Debug.Log("Process enemies!");
                StartCoroutine(ProcessEnemies());
            }
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
