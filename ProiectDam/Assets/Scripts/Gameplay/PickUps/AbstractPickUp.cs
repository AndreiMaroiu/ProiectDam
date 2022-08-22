using Gameplay.Player;

namespace Gameplay.PickUps
{
    public abstract class AbstractPickUp : IInteractableEnter
    {
        protected int _boost;

        public AbstractPickUp(int boost)
        {
            _boost = boost;
        }

        protected abstract void Interact(PlayerController controller);

        public void OnInteract(PlayerController controller)
        {
            if (controller is null)
            {
                return;
            }

            Interact(controller);
        }
    }
}
