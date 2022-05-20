using Gameplay.Player;
using UnityEngine;
using Values;

namespace Gameplay.PickUps
{
    public class MaxEnergyPickUp : BasePickUp
    {
        [SerializeField] private IntValue _boost;

        protected override void OnInteract(PlayerController controller)
        {
            controller.MaxEnergy += _boost;
        }
    }
}
