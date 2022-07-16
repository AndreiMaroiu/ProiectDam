using Gameplay.Generation;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.PickUps
{
    public class BasePickUp : TileObject, IInteractable
    {
        [SerializeField]
        private Item _item;

        public void Interact(PlayerController controller)
        {
            _item.GetPickUp().Interact(controller);

            Destroy(this.gameObject);
        }

        void IInteractable.OnPlayerLeave(PlayerController controller)
        {
            
        }
    }
}
