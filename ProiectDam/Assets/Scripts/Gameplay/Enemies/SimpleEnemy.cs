using Gameplay.Generation;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Enemies
{
    public sealed class SimpleEnemy : BaseEnemy
    {
        private const string WALK_ANIMATION = "Walk";
        private const string MELEE_ANIMATION = "Melee";
        private const string DEATH_ANIMATION = "Death";
        private const string HIT_ANIMATION = "Hit";

        private EnemySoundHandler _soundhandler;
        private Animator _animator;
        private SpriteRenderer _renderer;
        private Collider2D _collider;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _soundhandler = GetComponent<EnemySoundHandler>();
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
            InitHealth(_data.StartHealth);

            SetMove(_data.MoveTime, _data.CellSizeValue, TileType.Enemy);
        }

        protected override void OnDamage()
        {
            // play sound and animations
            _soundhandler.PlayHit();
            _animator.SetBool(HIT_ANIMATION, true);
        }

        private void OnStopDamage()
        {
            // play sound and animations
            _animator.SetBool(HIT_ANIMATION, false);
        }

        protected override void OnDeath()
        {
            _animator.SetBool(DEATH_ANIMATION, true);
            _collider.enabled = false;
        }

        public override void OnAttack(PlayerController player)
        {
            // play sounds and animations
            _soundhandler.PlayAttack();
            _animator.SetBool(MELEE_ANIMATION, true);

            _renderer.flipX = (player.transform.position.x - transform.position.x) < 0;
        }

        public void OnStopAttack()
        {
            // play sounds and animations
            _animator.SetBool(MELEE_ANIMATION, false);
        }

        protected override void OnMove(Vector2Int direction)
        {
            //play sounds and animations
            _renderer.flipX = direction.x < 0;
            _animator.SetBool(WALK_ANIMATION, true);
        }

        protected sealed override void OnStopMoving()
        {
           // _soundhandler.Stop();
            _animator.SetBool(WALK_ANIMATION, false);
        }
    }
}
