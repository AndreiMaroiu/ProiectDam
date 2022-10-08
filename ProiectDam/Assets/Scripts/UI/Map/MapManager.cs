using Core;
using Core.Events;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Core.Values;

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

        private void OnRoomChanged()
        {
            if (_currentRect.IsNotNull())
            {
                _currentRect.GetComponent<Image>().color = Color.gray;
            }

            RoomUIBehaviour room = _traverser[_currentRoom.Value.Pos];
            room.SetActive(Color.white);

            _currentRect = room;

            CheckBounds();
        }

        private void CheckBounds()
        {
            // TODO: refactor
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
            Room start = _currentRoom.StartRoom;
            _traverser = new RoomTraverser<RoomUIBehaviour>(start);

            _width = start.Pos;
            _height = start.Pos;

            // generating rooms
            _traverser.TraverseUnique(room =>
            {
                Vector2 pos = Vector2.zero;

                if (room.LastRoom != null)
                {
                    pos = _traverser[room.LastRoom.Pos].Rect.anchoredPosition + (Vector2)Utils.GetWorldDirection(room.Direction) * _cellDistance;
                }

                RoomUIBehaviour clone = Instantiate(_roomPrefab, _map.transform);

                clone.Rect.anchoredPosition = pos;
                clone.gameObject.SetActive(room.Discovered);
                clone.Set(_icons.GetIcon(room.Type));

                _traverser[room.Pos] = clone;
            });

            RectTransform door = GenerateDoor();

            // generating doors
            _traverser.Traverse(room =>
            {
                if (room.LastRoom is null)
                {
                    return;
                }

                RectTransform prevClone = Instantiate(door, _traverser[room.LastRoom.Pos].transform);
                prevClone.anchoredPosition = Utils.GetWorldDirection(room.Direction) * _cellSize / 2;

                RectTransform currentClone = Instantiate(door, _traverser[room.Pos].transform);
                currentClone.anchoredPosition = Utils.GetWorldDirection(room.Direction + 180) * _cellSize / 2;
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
