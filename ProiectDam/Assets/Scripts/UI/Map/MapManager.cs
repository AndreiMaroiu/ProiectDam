using Core;
using Core.Events;
using Core.Values;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI.Map
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private GameObject _map;
        [SerializeField] private float _cellSize;
        [SerializeField] private float _cellDistance;
        [SerializeField] private RoomUIBehaviour _roomPrefab;
        [SerializeField] private IntValue _matrixSize;
        [SerializeField] private MapIcons _icons;
        [Header("Events")]
        [SerializeField] private RoomEvent _currentRoom;

        private RoomTraverser<RoomUIBehaviour> _traverser;
        private RoomUIBehaviour _currentRect;
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

        private void OnRoomChanged(Room room = null)
        {
            room ??= _currentRoom;

            if (_currentRect.IsNotNull())
            {
                _currentRect.GetComponent<Image>().color = Color.gray;
            }

            RoomUIBehaviour behaviour = _traverser[room.Pos];
            behaviour.SetActive(Color.white);

            foreach (var item in room.Neighbours)
            {
                RoomUIBehaviour neighbour = _traverser[item.Pos];
                neighbour.SetActive(Color.black);
            }

            _currentRect = behaviour;

            CheckBounds();
        }

        private void CheckBounds()
        {
            // TODO: refactor
            Room room = _currentRoom.Value;
            float size = (_cellDistance + _cellSize);

            if (room.Pos.X > _width.x)
            {
                _mapRect.sizeDelta += new Vector2(0, size);
                _width.x = room.Pos.X;
            }
            else if (room.Pos.X < _width.y)
            {
                _mapRect.sizeDelta += new Vector2(0, size);
                _width.y = room.Pos.X;
            }
            else if (room.Pos.Y > _height.x)
            {
                _mapRect.sizeDelta += new Vector2(size, 0);
                _height.x = room.Pos.X;
            }
            else if (room.Pos.Y < _height.y)
            {
                _mapRect.sizeDelta += new Vector2(size, 0);
                _height.y = room.Pos.X;
            }
        }

        private void GenerateMap()
        {
            Room start = _currentRoom.StartRoom;
            _traverser = new RoomTraverser<RoomUIBehaviour>(start);

            _width = start.Pos;
            _height = start.Pos;

            // generating rooms
            _traverser.TraverseUnique(room =>
            {
                Vector2 temp = Utils.GetVector2FromMatrixPos(room.Pos - _traverser.GetStartRoom().Pos);
                Vector2 pos = temp * _cellDistance;

                RoomUIBehaviour clone = Instantiate(_roomPrefab, _map.transform);

                clone.Rect.anchoredPosition = pos;
                clone.gameObject.SetActive(room.Discovered);
                clone.Set(_icons.GetIcon(room.Type));

                _traverser[room.Pos] = clone;
            });

            // setting the preview room
            _traverser.TraverseUnique(room =>
            {
                if (room.Discovered)
                {
                    OnRoomChanged(room);
                }
            });

            RectTransform door = GenerateDoor();

            // generating doors
            _traverser.TraverseUnique(room =>
            {
                foreach (var neighbour in room)
                {
                    RectTransform currentClone = Instantiate(door, _traverser[room.Pos].transform);
                    currentClone.anchoredPosition = Utils.GetVector2FromMatrixPos(neighbour.Pos - room.Pos) * _cellSize / 2;
                }
            });

            OnRoomChanged();
            Destroy(door.gameObject);
        }

        private RectTransform GenerateDoor()
        {
            RectTransform rect = new GameObject("door image", typeof(RectTransform), typeof(Image))
                                    .GetComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.sizeDelta /= 5;

            rect.GetComponent<Image>().color = Color.grey;

            return rect;
        }
    }
}
