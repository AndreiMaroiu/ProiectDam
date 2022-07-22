using Gameplay.Player;

namespace Gameplay.PickUps
{
    public class BulletsBoost : AbstractPickUp
    {
        public BulletsBoost(int boost) : base(boost)
        {

        }

        protected override void Interact(PlayerController controller)
        {
            controller.Bullets += _boost;
        }
    }
}
