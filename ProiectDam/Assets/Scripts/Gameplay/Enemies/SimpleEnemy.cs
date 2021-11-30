using Gameplay.Generation;
using Gameplay.Player;
using UnityEngine;
using Values;

namespace Gameplay.Enemies
{
    public sealed class SimpleEnemy : BaseEnemy
    {
        [SerializeField] private int _startHealth = 1;
        [SerializeField] private float _moveTime;
        [SerializeField] private FloatValue _cellSizeValue;

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
        }

        public override void OnEnemyTurn(PlayerController player)
        {
            Vector2Int direction = Vector2Int.zero;

            Vector3 position = transform.position;
            Vector3 playerPosition = player.transform.position;

            if (Mathf.Abs(position.x - playerPosition.x) < float.Epsilon)
            {
                direction.y = position.y > playerPosition.y ? -1 : 1;
            }
            else
            {
                direction.x = position.x > playerPosition.x ? -1 : 1;
            }

            Debug.Log($"Position: {position.ToString()} Direction: {direction.ToString()} Player: {playerPosition.ToString()}");

            StartCoroutine(TryMove(direction));
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
