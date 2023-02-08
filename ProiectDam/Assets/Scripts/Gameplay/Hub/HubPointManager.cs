using Core;
using Core.Events;
using Core.Values;
using UnityEngine;
using Utilities;

namespace Gameplay.Hub
{
    public class HubPointManager : MonoBehaviour
    {
        [SerializeField] private HubPointEvent _hubPoint;
        [SerializeField] private HubPoint _startPoint;

        private readonly SwipeDetector _swipeDetector = new();

        private void Start()
        {
            _hubPoint.Value = _startPoint;
            _swipeDetector.OnSwipe += OnSwipe;

            _hubPoint.Value.Activate(true);
        }

        private void OnSwipe(Vector2Int direction)
        {
            if (_hubPoint.Value.CanMoveToDirection(direction))
            {
                _hubPoint.Value.Activate(false);
                _hubPoint.Value = _hubPoint.Value.GetPointInDirection(direction);
                _hubPoint.Value.Activate(true);
            }
        }

        void Update()
        {
            _swipeDetector.CkeckForSwipes();

            var direction = GetMoveDirection();
            if (direction != Vector2Int.zero)
            {
                OnSwipe(-direction);
            }
        }

        private Vector2Int GetMoveDirection()
        {
            Vector2Int dir = Vector2Int.zero;

            if (Input.GetKeyDown(KeyCode.A))
            {
                dir.x = -1;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                dir.x = 1;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                dir.y = 1;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                dir.y = -1;
            }

            return dir;
        }

        private void OnDestroy()
        {
            _hubPoint.Value = null;
        }
    }
}
