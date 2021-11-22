using Core;
using Events;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private GameObject _map;
        [SerializeField] private float _cellSize;
        [SerializeField] private float _cellDistance;
        [SerializeField] private RectTransform _roomPrefab;
        [Header("Events")]
        [SerializeField] private RoomEvent _currentRoom;

        private RoomTraverser<RectTransform> _traverser;
        private RectTransform _currentRect;

        private void Start()
        {
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
        }

        public void GenerateMap()
        {
            _traverser = new RoomTraverser<RectTransform>(_currentRoom.Value, 20);

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
