using Gameplay.Player;
using UnityEngine;
using Values;

namespace Gameplay.PickUps
{
    public class BulletsPickUp : BasePickUp
    {
        [SerializeField] private IntValue _bulletsBonus;

        protected override void OnInteract(PlayerController controller)
        {
            controller.Bullets += _bulletsBonus;
        }
    }
}
