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

        private void Awake()
        {
            _hubPoint.Value = _startPoint;
            _swipeDetector.OnSwipe += OnSwipe;
        }

        private void Start()
        {
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
        }

        private void OnDestroy()
        {
            _hubPoint.Value = null;
        }
    }
}
