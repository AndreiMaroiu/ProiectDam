using Core;
using Events;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Values;

namespace UI
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private GameObject _map;
        [SerializeField] private float _cellSize;
        [SerializeField] private float _cellDistance;
        [SerializeField] private RectTransform _roomPrefab;
        [SerializeField] private IntValue _matrixSize;
        [Header("Events")]
        [SerializeField] private RoomEvent _currentRoom;

        private RoomTraverser<RectTransform> _traverser;
        private RectTransform _currentRect;
        private RectTransform _mapRect;

        private Vector2Int _width;
        private Vector2Int _height;

        private void Start()
        {
            _mapRect = _map.GetComponent<RectTransform>();

            GenerateMap();

            _currentRoom.OnValueChanged += OnRoomChanged;
        }

        private void OnDestroy()
        {
            _currentRoom.OnValueChanged -= OnRoomChanged;
        }

        private void OnRoomChanged()
        {
            if (_currentRect.IsNotNull())
            {
                _currentRect.GetComponent<Image>().color = Color.gray;
            }

            RectTransform room = _traverser[_currentRoom.Value.Pos];
            room.gameObject.SetActive(true);
            room.GetComponent<Image>().color = Color.white;

            _currentRect = room;

            CheckMinMax();
        }

        private void CheckMinMax()
        {
            Room room = _currentRoom.Value;
            float size = (_cellDistance + _cellSize);

            if (room.Pos.x > _width.x)
            {
                _mapRect.sizeDelta += new Vector2(0, size);
                _width.x = room.Pos.x;
            }
            else if (room.Pos.x < _width.y)
            {
                _mapRect.sizeDelta += new Vector2(0, size);
                _width.y = room.Pos.x;
            }
            else if (room.Pos.y > _height.x)
            {
                _mapRect.sizeDelta += new Vector2(size, 0);
                _height.x = room.Pos.x;
            }
            else if (room.Pos.y < _height.y)
            {
                _mapRect.sizeDelta += new Vector2(size, 0);
                _height.y = room.Pos.x;
            }
        }

        private void GenerateMap()
        {
            Room start = _currentRoom.Value;
            _traverser = new RoomTraverser<RectTransform>(start, _matrixSize);

            _width = start.Pos;
            _height = start.Pos;

            _traverser.TraverseUnique(room =>
            {
                Vector2 pos = Vector2.zero;

                if (room.LastRoom != null)
                {
                    pos = _traverser[room.LastRoom.Pos].anchoredPosition + (Vector2)Utils.GetWorldDirection(room.Direction) * _cellDistance;
                }

                _traverser[room.Pos] = Instantiate(_roomPrefab, _map.transform);
                _traverser[room.Pos].anchoredPosition = pos;
                _traverser[room.Pos].gameObject.SetActive(false);
            });

            _traverser.Traverse(room =>
            {
                if (room.LastRoom is null)
                {
                    return;
                }

                RectTransform door = Instantiate(_roomPrefab, _traverser[room.LastRoom.Pos].transform);
                door.anchoredPosition = Utils.GetWorldDirection(room.Direction) * _cellSize / 2;
                door.sizeDelta /= 5;
            });

            OnRoomChanged();
        }
    }
}
