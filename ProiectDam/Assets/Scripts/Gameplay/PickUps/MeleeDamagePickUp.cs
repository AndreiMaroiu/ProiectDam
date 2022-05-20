using Gameplay.Player;
using UnityEngine;
using Values;

namespace Gameplay.PickUps
{
    public class MeleeDamagePickUp : BasePickUp
    {
        [SerializeField] private IntValue _damage;

        protected override void OnInteract(PlayerController controller)
        {
            controller.MeleeDamage += _damage;
        }
    }
}
