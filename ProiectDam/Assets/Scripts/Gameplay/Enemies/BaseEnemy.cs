using Gameplay.DataSaving;
using Gameplay.Generation;
using Gameplay.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Gameplay.Enemies
{
    public abstract class BaseEnemy : MovingObject, IDataSavingTile
    {
        private static readonly Vector2Int[] Directions = { Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.right };

        [SerializeField] protected EnemyData _data;

        private Vector2Int? _lastPosition;
        private Vector3 _targetPosition;

        public event Action<BaseEnemy> OnDeathEvent;

        public abstract void OnAttack(PlayerController player);

        public virtual IEnumerator OnEnemyTurn(PlayerController player, HashSet<Vector2Int> availablePositions)
        {
            if ((LayerPosition.Position - player.LayerPosition.Position).sqrMagnitude == 1)
            {
                player.TakeDamage(_data.Damage, this);
                OnAttack(player);
                yield break;
            }

            Vector2Int direction = -Utils.GetMatrixPos(GetMoveDirection(player, availablePositions));
            yield return TryMove(direction);
        }

        protected Vector2Int GetMoveDirection(PlayerController player, HashSet<Vector2Int> availablePositions)
        {
            TileType[,] layer = LayerPosition.Layer;

            float min = float.MaxValue;
            Vector2Int result = Vector2Int.zero;

            RandomPicker<Vector2Int> picker = new(Directions, createCopy: false);

            while (picker.CanTake)
            {
                Vector2Int direction = picker.Take();

                Vector2Int pos = direction + LayerPosition.Position;

                if (availablePositions.Contains(pos) || !layer[pos.x, pos.y].CanMove() || _lastPosition.HasValue && _lastPosition.Value == pos)
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
            availablePositions.Add(result);
            return result;
        }

        public override void OnDeathFinished()
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

        Guid IDataSavingTile.ObjectId { get; set; }

        public abstract ObjectSaveData SaveData { get; }

        protected Guid ObjectId => ((IDataSavingTile)this).ObjectId;

        void IDataSavingObject<ObjectSaveData>.LoadFromSave(ObjectSaveData data)
        {
            LoadFromSave(data);
        }

        protected abstract void LoadFromSave(ObjectSaveData data);

        #endregion
    }
}
