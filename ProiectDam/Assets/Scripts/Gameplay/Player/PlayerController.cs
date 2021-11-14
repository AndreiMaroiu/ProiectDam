using Gameplay.Generation;
using System.Collections;
using UnityEngine;
using Values;

namespace Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        private const string WALK_ANIMATION = "Walk";

        [SerializeField] private FloatValue _cellSize;
        [SerializeField] private float _moveTime = 1.0f;

        private Vector2 _direction;
        private Animator _animator;

        private bool _canMove = true;
        private float _inverseMoveTime;

        public LayerPosition LayerPosition { get; set; }

        private void Start()
        {
            _animator = GetComponent<Animator>();

            _inverseMoveTime = 1 / _moveTime;
        }

        public void Set(LayerPosition layerPosition)
        {
            LayerPosition = layerPosition;
        }

        private Vector2Int GetMoveDirection()
        {
            Vector2Int dir = Vector2Int.zero;

            if (Input.GetKeyDown(KeyCode.A))
            {
                dir.x = -1;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                dir.x = 1;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                dir.y = 1;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                dir.y = -1;
            }

            return dir;
        }

        private void Update()
        {
            if (_canMove)
            {
                Vector2Int dir = GetMoveDirection();
                if (dir.sqrMagnitude > Vector3.kEpsilon)
                {  
                    StartCoroutine(Move(dir));
                }
            }

            AnimatePlayer();
        }

        private IEnumerator Move(Vector2Int dir)
        {
            _canMove = false;
            Vector3 endPosition = transform.position + (new Vector3(dir.x, dir.y) * _cellSize.Value);
            _direction = dir;

            if (!LayerPosition.TryMove(new Vector2Int(dir.y, -dir.x)))
            {
                _canMove = true;
            }

            while (transform.position != endPosition && !_canMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosition, _inverseMoveTime * Time.deltaTime);
                yield return null;
            }

            _direction = Vector2.zero;
            _canMove = true;
        }

        private void AnimatePlayer()
        {
            bool isMoving = _direction.sqrMagnitude > 0;
            _animator.SetBool(WALK_ANIMATION, isMoving);

            float xScale = transform.localScale.x;
            bool shouldFlip = _direction.x > 0 && xScale < 0 || _direction.x < 0 && xScale > 0;

            if (shouldFlip)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, 
                    transform.localScale.y, transform.localScale.z);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();

            interactable?.Interact(this);
            _canMove = true;
        }
    }
}
