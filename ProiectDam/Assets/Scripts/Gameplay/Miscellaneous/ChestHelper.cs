using Gameplay.Player;
using UnityEngine;

namespace Gameplay
{
    public class ChestHelper : MonoBehaviour, IInteractable
    {
        private PlayerController _controller;

        public bool CanInteract { get; private set; }

        public void Interact(PlayerController controller)
        {
            CanInteract = true;
            _controller = controller;
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject == _controller.gameObject)
            {
                CanInteract = false;
            }
        }
    }
}
