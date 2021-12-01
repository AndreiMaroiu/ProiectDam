using Gameplay.Player;
using UnityEngine;

namespace Gameplay
{
    public class ChestHelper : MonoBehaviour, IInteractable
    {
        public bool CanInteract { get; private set; }
        public PlayerController Controller { get; private set; }

        public void Interact(PlayerController controller)
        {
            CanInteract = true;
            Controller = controller;
            Debug.Log("Can Open Chest!");
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject == Controller.gameObject)
            {
                CanInteract = false;
                Debug.Log("Cannot open chest anymore!");
            }
        }
    }
}
