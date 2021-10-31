using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        private const string WALK_ANIMATION = "Walk";

        [SerializeField] private float _speed;

        private Vector2 _direction;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private SpriteRenderer[] _renderers;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        private void Update()
        {
            _direction.x = Input.GetAxis("Horizontal");
            _direction.y = Input.GetAxis("Vertical");

            AnimatePlayer();
        }

        void AnimatePlayer()
        {
            bool isMoving = _direction.sqrMagnitude > 0;
            _animator.SetBool(WALK_ANIMATION, isMoving);

            bool isFlipped = _direction.x < 0;
            foreach (SpriteRenderer sprite in _renderers)
            {
                sprite.flipX = isFlipped;
            }
        }

        private void FixedUpdate()
        {
            _rigidbody.MovePosition(_rigidbody.position + (_direction * (Time.fixedDeltaTime * _speed)));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();

            interactable?.Interact(this);
        }
    }
}
