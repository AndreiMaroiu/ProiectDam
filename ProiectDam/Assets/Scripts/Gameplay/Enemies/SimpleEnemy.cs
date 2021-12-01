using Gameplay.Generation;
using UnityEngine;

namespace Gameplay.Enemies
{
    public sealed class SimpleEnemy : BaseEnemy
    {
        private EnemySoundHandler _soundhandler;

        private void Start()
        {
            _soundhandler = GetComponent<EnemySoundHandler>();
            MaxHealth = _startHealth;
            Health = _startHealth;

            SetMove(_moveTime, _cellSizeValue, TileType.Enemy);
        }

        protected override void OnDamage()
        {
            // play sound and animations
            _soundhandler.PlayHit();

            Debug.Log("Hit!");
        }

        public override void OnAttack()
        {
            // play sounds and animations
        }

        protected override void OnMove(Vector2Int direction)
        {
            //play sounds and animations
        }

        protected override void OnStopMoving()
        {
            _soundhandler.Stop();
        }

        protected override bool CanMoveToTile(TileType tile)
        {
            return tile.CanMove();
        }
    }
}
