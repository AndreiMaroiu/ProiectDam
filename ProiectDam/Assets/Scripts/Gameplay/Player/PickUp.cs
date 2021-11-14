using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player
{
    public class PickUp : BasePickUp
    {
        protected override void OnInteract(PlayerController controller)
        {
            Debug.Log("On Pick up");
        }
    }
}
