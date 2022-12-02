using Gameplay.Player;
using System.Collections;
using UnityEngine;
using Utilities;

namespace Gameplay.Enemies
{
    public class BossEnemy : SimpleEnemy
    {
        public override IEnumerator OnEnemyTurn(PlayerController player)
        {
            if ((LayerPosition.Position - player.LayerPosition.Position).sqrMagnitude == 1)
            {
                player.TakeDamage(_data.Damage);
                OnAttack(player);
                yield break;
            }

            Vector2Int direction = -Utils.GetMatrixPos(GetMoveDirection(player));
            yield return TryMove(direction);

            if ((LayerPosition.Position - player.LayerPosition.Position).sqrMagnitude > 1)
            {
                direction = -Utils.GetMatrixPos(GetMoveDirection(player));
                yield return TryMove(direction);
            }
        }
    }
}
