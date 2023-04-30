using Gameplay.DataSaving;
using Gameplay.Generation;
using Gameplay.Player;
using System.Collections;
using UnityEngine;
using Utilities;

namespace Gameplay.Enemies
{
    public class BossEnemy : SimpleEnemy
    {
        public enum BossPhase
        {
            One,
            Two,
            Three,
        }

        [SerializeField] private TileObject _keyPrefab;

        private BossPhase _phase = BossPhase.One;

        public event System.Action<BossPhase> OnPhaseChange;

        public override IEnumerator OnEnemyTurn(PlayerController player)
        {
            if (CanHit is false)
            {
                yield break;
            }

            if ((LayerPosition.Position - player.LayerPosition.Position).sqrMagnitude == 1)
            {
                player.TakeDamage(_data.Damage, this);
                OnAttack(player);
                yield break;
            }

            Vector2Int direction = -Utils.GetMatrixPos(GetMoveDirection(player));
            yield return TryMove(direction);

            if ((LayerPosition.Position - player.LayerPosition.Position).sqrMagnitude > 1 && Random.Range(0, 100) < 50)
            {
                direction = -Utils.GetMatrixPos(GetMoveDirection(player));
                yield return TryMove(direction);
            }
        }

        protected override void OnDamage(MonoBehaviour dealer)
        {
            base.OnDamage(dealer);

            if (Health <= 0)
            {
                return;
            }

            Teleport();

            if (_phase is BossPhase.One && Health <= MaxHealth / 2)
            {
                CanHit = false;
                _phase = BossPhase.Two;
                OnPhaseChange?.Invoke(BossPhase.Two);
                // wait for all enemies to get killed
            }
        }

        public override void OnDeathFinished()
        {
            base.OnDeathFinished();

            TileObject tile = Instantiate(_keyPrefab);
            tile.transform.position = transform.position;
            tile.LayerPosition = new(LayerPosition);
            tile.LayerPosition.TileType = TileType.PickUp;
        }

        private void Teleport()
        {
            int x;
            int y;

            Vector2Int target;

            do
            {
                x = Random.Range(2, 5) * (int)Mathf.Pow(-1, Random.Range(0, 2));
                y = Random.Range(2, 5) * (int)Mathf.Pow(-1, Random.Range(0, 2));
                target = LayerPosition.Position + new Vector2Int(x, y);
            } while (IsInBounds(target) is false || LayerPosition.Layer[target.x, target.y] is not TileType.None);

            Vector2Int direction = new(x, y);

            Vector3 movePos = transform.position + Utils.GetVector3FromMatrixPos(direction, CellSize);
            transform.position = movePos;
            LayerPosition.TileType = TileType.None;
            LayerPosition.Move(direction);
            LayerPosition.TileType = TileType.Enemy;
        }

        private bool IsInBounds(Vector2Int target)
        {
            return target.x >= 0 && target.x < LayerPosition.Layer.GetLength(0) &&
                    target.y >= 0 && target.y < LayerPosition.Layer.GetLength(0);
        }

        internal void AdvancePhase()
        {
            CanHit = true;
            _phase = BossPhase.Three;
        }

        #region Data Saving

        protected override void LoadFromSave(ObjectSaveData data)
        {
            base.LoadFromSave(data);

            if (data is BossEnemySaveData bossData)
            {
                _phase = bossData.Phase;
            }
        }

        public override ObjectSaveData SaveData => CreateBossSaveData();

        private BossEnemySaveData CreateBossSaveData()
        {
            var data = CreateSaveData();

            return new BossEnemySaveData()
            {
                CanHit = data.CanHit,
                Health = data.Health,
                IsFlipped = data.IsFlipped,
                LastTile = data.LastTile,
                ObjectId = data.ObjectId,
                Phase = _phase,
            };
        }

        #endregion
    }
}
