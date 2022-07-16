using Gameplay.Player;

namespace Gameplay.PickUps
{
    public class BulletsBoost : AbstractPickUp
    {
        public BulletsBoost(int boost) : base(boost)
        {

        }

        public override void Interact(PlayerController controller)
        {
            controller.Bullets += _boost;
        }
    }
}
