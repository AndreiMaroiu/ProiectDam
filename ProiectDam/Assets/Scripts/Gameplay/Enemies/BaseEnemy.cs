using Gameplay.DataSaving;
using Gameplay.Generation;
using Gameplay.Player;
using System;
using System.Collections;
using UnityEngine;
using Utilities;

namespace Gameplay.Enemies
{
    public abstract class BaseEnemy : MovingObject, IDataSavingObject
    {
        private static readonly Vector2Int[] Directions = { Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.right };

        [SerializeField] protected EnemyData _data;

        private Vector2Int? _lastPosition;
        private Vector3 _targetPosition;

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
        }

        public sealed override void OnDeathFinished()
        {
            OnDeathEvent?.Invoke(this);
            LayerPosition?.Clear();
            Destroy(this.gameObject);
            _data.GlobalDeathEvent.Invoke(this);
        }

        protected override void OnMove(Vector2Int direction)
        {
            _targetPosition = transform.position + new Vector3(direction.x, direction.y) * CellSize;
        }

        protected sealed override bool CanMoveToTile(TileType tile)
        {
            return tile.CanMove();
        }

        public void OnDrawGizmos()
        {
            if (IsMoving)
            {
                Color lastColor = Gizmos.color;
                Gizmos.color = Color.red;

                Gizmos.DrawCube(_targetPosition, CellSize * Vector3.one);

                Gizmos.color = lastColor;
            }
        }

        #region IDataSavingObject

        string IDataSavingObject.ObjectName { get; set; }
        public abstract ObjectSaveData SaveData { get; }

        protected string ObjectName => ((IDataSavingObject)this).ObjectName;

        void IDataSavingObject.LoadFromSave(ObjectSaveData data)
        {
            LoadFromSave(data);
        }

        protected abstract void LoadFromSave(ObjectSaveData data);

        #endregion
    }
}
