using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using Core;
using Utilities;
using UnityEngine.UI;

namespace UI
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private GameObject _map;
        [SerializeField] private float _cellSize;
        [SerializeField] private float _cellDistance;
        [SerializeField] private RectTransform _roomPrefab;
        [Header("Events")]
        [SerializeField] private RoomEvent _currentRoom;

        private RoomTraverser<RectTransform> _traverser;

        private void Start()
        {
            _currentRoom.OnValueChanged += OnRoomChanged;

            _traverser = new RoomTraverser<RectTransform>(_currentRoom.Value, 20);

            GenerateMap();

            OnRoomChanged();
        }

        private void OnDestroy()
        {
            _currentRoom.OnValueChanged -= OnRoomChanged;
        }

        private void OnRoomChanged()
        {
            // change active room

            _traverser[_currentRoom.Value.Pos].GetComponent<Image>().color = Color.white;
            _traverser[_currentRoom.Value.Pos].SetAsLastSibling();
        }

        private void GenerateMap()
        {
            _traverser.TraverseUnique(room =>
            {
                Vector2 pos = Vector2.zero;

                if (room.LastRoom != null)
                {
                    pos = _traverser[room.LastRoom.Pos].anchoredPosition + (Vector2)Utils.GetWorldDirection(room.Direction) * _cellDistance;
                }

                _traverser[room.Pos] = Instantiate(_roomPrefab, _map.transform);
                _traverser[room.Pos].anchoredPosition = pos;
            });

            _traverser.Traverse(room =>
            {
                Vector2 pos = Vector2.zero;

                if (room.LastRoom != null)
                {
                    pos = _traverser[room.LastRoom.Pos].anchoredPosition + (Vector2)Utils.GetWorldDirection(room.Direction) * _cellSize / 2;
                }

                RectTransform door = Instantiate(_roomPrefab, _map.transform);
                door.anchoredPosition = pos;
                door.sizeDelta /= 2;
            });
        }
    }
}
