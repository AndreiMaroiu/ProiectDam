using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player
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
