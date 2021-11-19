using Gameplay.Generation;
using System.Collections;
using UnityEngine;
using Values;
using Events;

namespace Gameplay.Player
{
    public class PlayerController : KillableObject
    {
        private const string WALK_ANIMATION = "Walk";

        [SerializeField] private FloatValue _cellSize;
        [SerializeField] private float _moveTime = 1.0f;
        [SerializeField] private int _startEnergy;
        [SerializeField] private int _startHealth;
        [Header("Events")]
        [SerializeField] private CappedIntEvent _bulletsEvent;
        [SerializeField] private CappedIntEvent _healthEvent;
        [SerializeField] private CappedIntEvent _energyEvent;
        [SerializeField] private GameEvent _onPlayerDeath;
        [SerializeField] private BoolEvent _previewEvent;

        private Vector2 _direction;
        private Animator _animator;

        private bool _canMove = true;
        private float _inverseMoveTime;

        public override int Health 
        { 
            get => _healthEvent.Value; 
            set => _healthEvent.Value = value; 
        }

        public override int MaxHealth 
        { 
            get => _healthEvent.MaxValue; 
            set => _healthEvent.MaxValue = value; 
        }

        public int Bullets
        {
            get => _bulletsEvent.Value;
            set => _bulletsEvent.Value = value;
        }

        public int MaxBullets
        {
            get => _bulletsEvent.MaxValue;
            set => _bulletsEvent.MaxValue = value;
        }

        public int Energy
        {
            get => _energyEvent.Value;
            set => _energyEvent.Value = value;
        }

        public int MaxEnergy
        {
            get => _energyEvent.MaxValue;
            set => _energyEvent.MaxValue = value;
        }

        public LayerPosition LayerPosition { get; set; }

        #region Unity Events

        private void Awake()
        {
            _energyEvent.Init(_startEnergy, _startEnergy);
            _healthEvent.Init(_startHealth, _startHealth);

            _previewEvent.OnValueChanged += OnPreviewChanged;
        }

        private void OnDestroy()
        {
            _previewEvent.OnValueChanged -= OnPreviewChanged;
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _inverseMoveTime = 1 / _moveTime;
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();

            interactable?.Interact(this);
        }

        #endregion

        private void OnPreviewChanged()
        {
            gameObject.SetActive(_previewEvent.Value);
        }

        public void StopMoving() 
            => _canMove = true;

        private void OnMove()
        {
            _energyEvent.Value--;

            if (_energyEvent.Value <= 0)
            {
                OnDeath();
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

        private IEnumerator Move(Vector2Int dir)
        {
            if (!LayerPosition.TryMove(new Vector2Int(dir.y, -dir.x)))
            {
                yield break;
            }

            Vector3 endPosition = transform.position + (new Vector3(dir.x, dir.y) * _cellSize.Value);
            _canMove = false;
            _direction = dir;
            OnMove();

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

        protected override void OnDamage()
        {
            
        }

        protected override void OnDeath()
        {
            _onPlayerDeath.Invoke();
        }
    }
}
