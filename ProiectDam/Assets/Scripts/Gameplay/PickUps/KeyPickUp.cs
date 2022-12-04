using Gameplay.Player;

namespace Gameplay.PickUps
{
    public class KeyPickUp : AbstractPickUp
    {
        public KeyPickUp(int boost) : base(boost)
        {

        }

        protected override void Interact(PlayerController controller)
        {
            controller.Keys += _boost;
        }
    }
}
