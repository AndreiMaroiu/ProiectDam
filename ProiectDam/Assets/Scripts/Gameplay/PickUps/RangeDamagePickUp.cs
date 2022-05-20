using Gameplay.Player;
using UnityEngine;
using Values;

namespace Gameplay.PickUps
{
    public class RangeDamagePickUp : BasePickUp
    {
        [SerializeField] private IntValue _damage;

        protected override void OnInteract(PlayerController controller)
        {
            controller.RangedDamage += _damage;
        }
    }
}
