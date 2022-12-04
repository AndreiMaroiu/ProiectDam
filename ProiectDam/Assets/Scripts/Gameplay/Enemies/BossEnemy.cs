using Gameplay.Generation;
using Gameplay.Player;
using System.Collections;
using UnityEngine;
using Utilities;

namespace Gameplay.Enemies
{
    public class BossEnemy : SimpleEnemy
    {
        [SerializeField] private TileObject _keyPrefab;

        public override IEnumerator OnEnemyTurn(PlayerController player)
        {
            if ((LayerPosition.Position - player.LayerPosition.Position).sqrMagnitude == 1)
            {
                player.TakeDamage(_data.Damage, this);
                OnAttack(player);
                yield break;
            }

            Vector2Int direction = -Utils.GetMatrixPos(GetMoveDirection(player));
            yield return TryMove(direction);

            if ((LayerPosition.Position - player.LayerPosition.Position).sqrMagnitude > 0 && Random.Range(0, 100) < 50)
            {
                direction = -Utils.GetMatrixPos(GetMoveDirection(player));
                yield return TryMove(direction);
            }
        }

        protected override void OnDamage(MonoBehaviour dealer)
        {
            base.OnDamage(dealer);

            if (dealer is not PlayerController player || Health <= 0)
            {
                return;
            }

            Vector2Int direction = LayerPosition.Position - player.LayerPosition.Position;

            if (!CanMoveToTile(LayerPosition.GetTile(direction)))
            {
                return;
            }

            Vector3 movePos = transform.position + Utils.GetVector3FromMatrixPos(direction, CellSize);
            transform.position = movePos;
            LayerPosition.TileType = TileType.None;
            LayerPosition.Move(direction);
            LayerPosition.TileType = TileType.Enemy;
        }

        public override void OnDeathFinished()
        {
            base.OnDeathFinished();

            TileObject tile = Instantiate(_keyPrefab);
            tile.transform.position = transform.position;
            tile.LayerPosition = new(LayerPosition);
            tile.LayerPosition.TileType = TileType.PickUp;
        }
    }
}
