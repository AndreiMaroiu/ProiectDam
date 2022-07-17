using Gameplay.Generation;
using Gameplay.Player;
using System;
using UnityEngine;
using Utilities;
using Core.Values;
using Priority_Queue;
using System.Collections.Generic;

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

        public abstract void OnAttack(PlayerController player);

        public void OnEnemyTurn(PlayerController player)
        {
            if ((LayerPosition.Position - player.LayerPosition.Position).sqrMagnitude == 1)
            {
                player.TakeDamage(_damage);
                OnAttack(player);
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

            //var queue = new SimplePriorityQueue<Vector2Int, int>();
            //var previous = new Dictionary<Vector2Int, Vector2Int>();
            //var visited = new HashSet<Vector2Int>();
            //var playerPos = player.LayerPosition.Position;

            //TileType[,] layer = LayerPosition.Layer;


            ////float min = float.MaxValue;
            //Vector2Int result = Vector2Int.zero;

            //queue.Enqueue(LayerPosition.Position, 0);

            //// TODO: save data for backtracking
            //bool found = false;

            //while (queue.Count > 0)
            //{
            //    var first = queue.Dequeue();

            //    visited.Add(first);

            //    foreach (Vector2Int direction in Directions)
            //    {
            //        Vector2Int pos = direction + first;

            //        if (layer[pos.x, pos.y] == TileType.Player)
            //        {
            //            previous[pos] = first;
            //            found = true;
            //        }

            //        if (!layer[pos.x, pos.y].CanMove() || visited.Contains(pos))
            //        {
            //            continue;
            //        }

            //        int distance = Math.Abs(pos.x - playerPos.x) + Math.Abs(pos.y - playerPos.y);

            //        queue.Enqueue(pos, distance);
            //        previous[pos] = first;                    
            //    }
            //}

            //if (!found)
            //{
            //    print("not found direction");
            //    return Vector2Int.zero;
            //}

            //Vector2Int current = player.LayerPosition.Position;

            //while (previous[current] != this.LayerPosition.Position)
            //{
            //    current = previous[current];
            //}

            //return current - this.LayerPosition.Position;
        }

        public sealed override void OnDeathFinished()
        {
            OnDeathEvent?.Invoke(this);
            LayerPosition?.Clear();
            Destroy(this.gameObject);
        }

        protected sealed override bool CanMoveToTile(TileType tile)
        {
            return tile.CanMove();
        }

        private void OnDrawGizmos()
        {
            
        }
    }
}
