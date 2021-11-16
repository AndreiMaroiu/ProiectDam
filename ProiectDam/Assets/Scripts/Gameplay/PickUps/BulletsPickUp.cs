using Gameplay.Player;
using UnityEngine;

namespace Gameplay.PickUps
{
    public class BulletsPickUp : BasePickUp
    {
        [SerializeField] private int _bulletsBonus;

        protected override void OnInteract(PlayerController controller)
        {
            controller.Bullets += _bulletsBonus;
        }
    }
}
