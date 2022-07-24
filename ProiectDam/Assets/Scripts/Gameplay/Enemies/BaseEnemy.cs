using Gameplay.Generation;
using Gameplay.Player;
using System;
using System.Collections;
using UnityEngine;
using Utilities;

namespace Gameplay.Enemies
{
    public abstract class BaseEnemy : MovingObject
    {
        private static readonly Vector2Int[] Directions = { Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.right };

        [SerializeField] protected EnemyData _data;

        private Vector2Int? _lastPosition;

        public event Action<BaseEnemy> OnDeathEvent;

        public abstract void OnAttack(PlayerController player);

        public IEnumerator OnEnemyTurn(PlayerController player)
        {
            if ((LayerPosition.Position - player.LayerPosition.Position).sqrMagnitude == 1)
            {
                player.TakeDamage(_data.Damage);
                OnAttack(player);
                yield break;
            }

            Vector2Int direction = -Utils.GetMatrixPos(GetMoveDirection(player));
            yield return TryMove(direction);
        }

        private Vector2Int GetMoveDirection(PlayerController player)
        {
            TileType[,] layer = LayerPosition.Layer;

            float min = float.MaxValue;
            Vector2Int result = Vector2Int.zero;

            foreach (Vector2Int direction in Directions)
            {
                Vector2Int pos = direction + LayerPosition.Position;

                if (!layer[pos.x, pos.y].CanMove() || _lastPosition.HasValue && _lastPosition.Value == pos)
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

            _lastPosition = LayerPosition.Position;

            return result;

            //var queue = new SimplePriorityQueue<Vector2Int, int>();
            //var previous = new Dictionary<Vector2Int, Vector2Int>();
            //var visited = new HashSet<Vector2Int>();
            //var playerPos = player.LayerPosition.Position;
            //var layer = LayerPosition.Layer;
            //var found = false;

            //queue.Enqueue(LayerPosition.Position, 0);

            ////if (_lastPosition.HasValue)
            ////{
            ////    visited.Add(_lastPosition.Value);
            ////}

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
            //    print("no direction found!");
            //    return Vector2Int.zero;
            //}

            //Vector2Int current = player.LayerPosition.Position;
            //_lastPosition = current;

            //while (previous[current] != this.LayerPosition.Position)
            //{
            //    current = previous[current];
            //}

            //Vector2Int result = current - this.LayerPosition.Position;
            //return result;
        }

        public sealed override void OnDeathFinished()
        {
            OnDeathEvent?.Invoke(this);
            LayerPosition?.Clear();
            Destroy(this.gameObject);
            _data.GlobalDeathEvent.Invoke(this);
        }

        protected sealed override bool CanMoveToTile(TileType tile)
        {
            return tile.CanMove();
        }
    }
}
