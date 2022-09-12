using Gameplay.DataSaving;
using Gameplay.Generation;
using Gameplay.Player;
using UnityEngine;
using Utilities;

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

        public void OnStopDamage()
        {
            // play sound and animations
            _animator.SetBool(HIT_ANIMATION, false);
        }

        protected override void OnDeath()
        {
            _animator.SetBool(HIT_ANIMATION, false);
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
            base.OnMove(direction);

            //play sounds and animations
            _renderer.flipX = direction.x < 0;
            _animator.SetBool(WALK_ANIMATION, true);
        }

        protected sealed override void OnStopMoving()
        {
           // _soundhandler.Stop();
            _animator.SetBool(WALK_ANIMATION, false);
        }

        private void LoadSpriteRenderer()
        {
            if (_renderer.IsNull())
            {
                _renderer = GetComponent<SpriteRenderer>();
            }
        }

        #region Data Saving

        public override ObjectSaveData SaveData => CreateSaveData();

        protected override void LoadFromSave(ObjectSaveData data)
        {
            if (data is SimpleEnemySaveData enemyData)
            {
                LoadSpriteRenderer();

                Health = enemyData.Health;
                _renderer.flipX = enemyData.IsFlipped;
                _lastType = enemyData.LastTile;
            }
        }

        private SimpleEnemySaveData CreateSaveData()
        {
            LoadSpriteRenderer();

            return new SimpleEnemySaveData()
            {
                ObjectName = this.ObjectName,
                Health = this.Health,
                IsFlipped = _renderer.flipX,
                LastTile = _lastType,
            };
        }

        #endregion
    }
}
