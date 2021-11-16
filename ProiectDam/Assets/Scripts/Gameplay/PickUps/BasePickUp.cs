using Gameplay.Player;
using UnityEngine;

namespace Gameplay.PickUps
{
    public abstract class BasePickUp : MonoBehaviour, IInteractable
    {
        public void Interact(PlayerController controller)
        {
            OnInteract(controller);
            Destroy(this.gameObject);
        }

        protected abstract void OnInteract(PlayerController controller);
    }
}
