using Events;
using Gameplay.Generation;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Values;

namespace Gameplay.Player
{
    public class PlayerController : MovingObject
    {
        private static readonly Vector2[] Directions = { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
        private static readonly Vector2[] ReversedDirections = { Vector2.left, Vector2.up, Vector2.right, Vector2.down };
        private const string WALK_ANIMATION = "Walk";
        private const string MELEE_ANIMATION = "Melee";
        private const string SHOOT_ANIMATION = "Shoot";
        private const string DEATH_ANIMATION = "Death";

        [SerializeField] private FloatValue _cellSizeValue;
        [SerializeField] private float _moveTime = 0.3f;
        [SerializeField] private int _startEnergy;
        [SerializeField] private int _startHealth;
        [SerializeField] private int _startBullets;
        [SerializeField] private int _meleeDamage;
        [SerializeField] private int _rangedDamage;
        [SerializeField] private LayerMask _mask;
        [Header("Colors")]
        [SerializeField] private Color _transparent;
        [SerializeField] private Color _red;
        [SerializeField] private Color _green;
        [Header("Events")]
        [SerializeField] private CappedIntEvent _bulletsEvent;
        [SerializeField] private CappedIntEvent _healthEvent;
        [SerializeField] private CappedIntEvent _energyEvent;
        [SerializeField] private GameEvent _onPlayerDeath;
        [SerializeField] private GameEvent _onMeleeAttack;
        [SerializeField] private GameEvent _onRangeAttack;

        private Vector2 _direction;
        private Animator _animator;
        private SoundHandler _soundhandler;
        private SpriteRenderer[] _renderers;
        private Collider2D _collider;

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
            _energyEvent.Init(_startEnergy);
            _healthEvent.Init(_startHealth);
            _bulletsEvent.Init(_startBullets);
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _soundhandler = GetComponent<SoundHandler>();
            _renderers = GetComponentsInChildren<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();

            _swipeDetector = new SwipeDetector();
            _swipeDetector.OnSwipe += OnSwipe;

            _onMeleeAttack.OnEvent += OnMeleeAttack;
            _onRangeAttack.OnEvent += OnRangedAttack;

            SetMove(_moveTime, _cellSizeValue.Value, Generation.TileType.Player);
        }

        private void OnDestroy()
        {
            _onMeleeAttack.OnEvent -= OnMeleeAttack;
            _onRangeAttack.OnEvent -= OnRangedAttack;
        }

        private void Update()
        {
            if (Time.timeScale == 0.0f)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                _onMeleeAttack.Invoke();
            }

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

        private void OnMeleeAttack()
        {
            foreach (KillableObject enemy in GetNearbyEnemies(Directions, _cellSizeValue.Value))
            {
                enemy.TakeDamage(_meleeDamage);
            }

            // play melee attack animation and sound
            _animator.SetBool(MELEE_ANIMATION, true);

        }

        private void OnMeleeEnd()
        {
            _animator.SetBool(MELEE_ANIMATION, false);
        }
        private void OnShootEnd()
        {
            _animator.SetBool(SHOOT_ANIMATION, false);
        }

        private void OnRangedAttack()
        {
            if (_bulletsEvent.Value <= 0)
            {
                // play sound for empty magazin
                return;
            }

            // play shoot animation and sound

            --_bulletsEvent.Value;
            _animator.SetBool(SHOOT_ANIMATION, true);

            Vector2[] directions = transform.localScale.x > 0 ? Directions : ReversedDirections;
            List<KillableObject> enemies = GetNearbyEnemies(directions, _cellSizeValue.Value * 2);

            float min = float.MaxValue;
            KillableObject closest = null;
            foreach (KillableObject enemy in enemies)
            {
                float distance = (enemy.transform.position - transform.position).sqrMagnitude;
                if (distance < min)
                {
                    min = distance;
                    closest = enemy;
                }
            }

            if (closest.IsNotNull())
            {
                closest.TakeDamage(_rangedDamage);

                float xPlayer = transform.position.x;
                float XEnemy = closest.transform.position.x;
                float xScale = transform.localScale.x;
                bool shouldFlip = xScale > 0 && XEnemy < xPlayer || xScale < 0 && XEnemy > xPlayer;

                if (shouldFlip)
                {
                    Vector3 scale = transform.localScale;
                    transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
                }
            }
        }

        private List<KillableObject> GetNearbyEnemies(Vector2[] directions, float distance)
        {
            List<KillableObject> result = new List<KillableObject>();

            _collider.enabled = false;

            foreach (Vector2 direction in directions)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, _mask);

                if (!hit)
                {
                    continue;
                }

                KillableObject enemy = hit.collider.gameObject.GetComponent<KillableObject>();

                if (enemy.IsNotNull())
                {
                    result.Add(enemy);
                }
            }

            _collider.enabled = true;

            return result;
        }

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
                Vector3 scale = transform.localScale;
                transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
            }
        }

        #region Overrides

        protected override void OnDamage()
        {

        }

        protected override void OnDeath()
        {
            _swipeDetector.OnSwipe -= OnSwipe;
            _animator.SetBool(DEATH_ANIMATION, true);
        }

        protected override void OnDeathFinished()
        {
            _onPlayerDeath.Invoke();
        }

        protected override bool CanMoveToTile(TileType tile)
        {
            return tile.CanMovePlayer();
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
            _soundhandler.Stop();
        }

        #endregion
    }
}
