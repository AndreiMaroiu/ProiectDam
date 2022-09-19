using Gameplay.Generation;
using Gameplay.Player;
using UnityEngine;
using Gameplay.DataSaving;

namespace Gameplay
{
    public class ChestHelper : MonoBehaviour, IInteractableEnter, IInteractableLeave
    {
        public bool CanInteract { get; private set; }
        public PlayerController Controller { get; private set; }

        public void OnInteract(PlayerController controller)
        {
            CanInteract = true;
            Controller = controller;
            Debug.Log("Can Open Chest!");
        }

        public void OnPlayerLeave(PlayerController controller)
        {
            CanInteract = false;
            Debug.Log("Cannot open chest anymore!");
        }
    }
}
