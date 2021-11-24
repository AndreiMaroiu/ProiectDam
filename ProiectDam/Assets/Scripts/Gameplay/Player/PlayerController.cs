using Events;
using Gameplay.Sound;
using UnityEngine;
using Values;
using Utilities;

namespace Gameplay.Player
{
    public class PlayerController : MovingObject
    {
        private const string WALK_ANIMATION = "Walk";

        [SerializeField] private FloatValue _cellSizeValue;
        [SerializeField] private float _moveTime = 1.0f;
        [SerializeField] private int _startEnergy;
        [SerializeField] private int _startHealth;
        [Header("Colors")]
        [SerializeField] private Color _transparent;
        [SerializeField] private Color _red;
        [SerializeField] private Color _green;
        [Header("Events")]
        [SerializeField] private CappedIntEvent _bulletsEvent;
        [SerializeField] private CappedIntEvent _healthEvent;
        [SerializeField] private CappedIntEvent _energyEvent;
        [SerializeField] private GameEvent _onPlayerDeath;

        private Vector2 _direction;
        private Animator _animator;
        private SoundHandler _soundhandler;
        private SpriteRenderer[] _renderers;

        private SwipeDetector _swipeDetector;

        #region Properties

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

        #endregion

        #region Unity Events

        private void Awake()
        {
            _energyEvent.Init(_startEnergy, _startEnergy);
            _healthEvent.Init(_startHealth, _startHealth);
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _soundhandler = GetComponent<SoundHandler>();
            _renderers = GetComponentsInChildren<SpriteRenderer>();

            _swipeDetector = new SwipeDetector();
            _swipeDetector.OnSwipe += OnSwipe;

            SetMove(_moveTime, _cellSizeValue.Value, Generation.TileType.Player);
        }

        private void Update()
        {
            _swipeDetector.CkeckForSwipes();

            if (CanMove)
            {
                Vector2Int dir = GetMoveDirection();
                if (dir.sqrMagnitude > Vector3.kEpsilon)
                {
                    StartCoroutine(TryMove(dir));
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

        #region Colors

        public void MakePlayerTransparent() => ChangeColor(_transparent);
        public void MakePlayerGreen() => ChangeColor(_green);
        public void MakePlayerRed() => ChangeColor(_red);
        public void MakePlayerWhite() => ChangeColor(Color.white);

        private void ChangeColor(Color color)
        {
            if (_renderers is null)
            {
                return;
            }

            foreach (SpriteRenderer spriteRenderer in _renderers)
            {
                spriteRenderer.color = color;
            }
        }

        #endregion

        private void OnSwipe(Vector2Int dir)
        {
            if (CanMove)
            {
                StartCoroutine(TryMove(dir));
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

        protected override void OnMove(Vector2Int direction)
        {
            _direction = direction;
            _energyEvent.Value--;

            if (_energyEvent.Value <= 0)
            {
                OnDeath();
            }
        }

        protected override void OnStopMoving()
        {
            _direction = Vector2.zero;
            _soundhandler.StopMove();
        }
    }
}
