using Gameplay.Generation;
using Gameplay.Player;
using System;
using UnityEngine;
using Utilities;
using Values;

namespace Gameplay.Enemies
{
    public abstract class BaseEnemy : MovingObject
    {
        private static readonly Vector2Int[] Directions = { Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.right };

        [SerializeField] protected int _damage;
        [SerializeField] protected int _startHealth = 1;
        [SerializeField] protected float _moveTime;
        [SerializeField] protected FloatValue _cellSizeValue;

        public event Action<BaseEnemy> OnDeathEvent;

        public abstract void OnAttack();

        public void OnEnemyTurn(PlayerController player)
        {
            if ((LayerPosition.Position - player.LayerPosition.Position).sqrMagnitude == 1)
            {
                player.TakeDamage(_damage);
                OnAttack();
                return;
            }

            Vector2Int direction = Utils.GetMatrixPos(GetMoveDirection(player));
            StartCoroutine(TryMove(-direction));
        }

        private Vector2Int GetMoveDirection(PlayerController player)
        {
            TileType[,] layer = LayerPosition.Layer;

            float min = float.MaxValue;
            Vector2Int result = Vector2Int.zero;

            foreach (Vector2Int direction in Directions)
            {
                Vector2Int pos = direction + LayerPosition.Position;
                if (!layer[pos.x, pos.y].CanMove())
                {
                    continue;
                }

                float distance = (pos - player.LayerPosition.Position).sqrMagnitude;
                if (distance < min)
                {
                    min = distance;
                    result = direction;
                }
            }

            return result;
        }

        protected sealed override void OnDeath()
        {
            OnDeathEvent?.Invoke(this);
            LayerPosition?.Clear();
            Destroy(this.gameObject);
        }
    }
}
