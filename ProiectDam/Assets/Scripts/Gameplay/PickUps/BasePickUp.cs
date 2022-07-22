using Gameplay.Generation;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.PickUps
{
    public class BasePickUp : TileObject, IInteractable
    {
        [SerializeField]
        private Item _item;

        public void OnInteract(PlayerController controller)
        {
            _item.GetPickUp().OnInteract(controller);

            Destroy(this.gameObject);
        }

        void IInteractable.OnPlayerLeave(PlayerController controller)
        {
            
        }
    }
}
