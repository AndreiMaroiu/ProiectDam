using Gameplay.Generation;
using Gameplay.Player;

namespace Gameplay.PickUps
{
    public abstract class BasePickUp : TileObject, IInteractable
    {
        public void Interact(PlayerController controller)
        {
            OnInteract(controller);

            Destroy(this.gameObject);
        }

        void IInteractable.OnPlayerLeave(PlayerController controller)
        {
            
        }

        protected abstract void OnInteract(PlayerController controller);
    }
}
