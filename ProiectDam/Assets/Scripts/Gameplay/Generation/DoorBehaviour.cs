using Gameplay.Events;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Generation
{
    public class DoorBehaviour : TileObject, IInteractableEnter
    {
        [SerializeField] private RoomBehaviourEvent _event;
        [Header("Sprites")]
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Sprite _openSprite;
        [SerializeField] private Sprite _closedSprite;

        private bool _isLocked = false;

        public bool IsLocked
        {
            get => _isLocked;

            set
            {
                _isLocked = value;

                if (value)
                {
                    OnClose();
                }
                else
                {
                    OnOpen();
                }
            }
        }

        private DoorBehaviour _other;
        private RoomBehaviour _room;
        private Vector3 _movePoint;
        private LayerPosition _moveLayerPosition;

        public void Set(Vector3 movePoint, DoorBehaviour other, RoomBehaviour room, LayerPosition layerPos)
        {
            _room = room;
            _other = other;
            _movePoint = movePoint;
            _moveLayerPosition = layerPos;

            OnOpen();
        }

        private void Move(Transform transform)
        {
            transform.position = _other._movePoint;
            _event.Value = _other._room;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(_movePoint, transform.localScale);
        }

        void IInteractableEnter.OnInteract(PlayerController controller)
        {
            if (IsLocked)
            {
                controller.transform.position = _movePoint;
                controller.StopMoving();
                controller.LayerPosition = new(_moveLayerPosition);
                return;
            }

            Move(controller.transform);
            controller.StopMoving();
            controller.LayerPosition = new(_other._moveLayerPosition)
            {
                TileType = TileType.Player
            };
        }

        private void OnClose()
        {
            _renderer.sprite = _closedSprite;

            if (LayerPosition is not null)
            {
                LayerPosition.TileType = TileType.Wall;
            }
        }

        private void OnOpen()
        {
            _renderer.sprite = _openSprite;

            if (LayerPosition is not null)
            {
                LayerPosition.TileType = TileType.Door;
            }
        }
    }
}
