using System;
using UnityEngine;

namespace Gameplay.PickUps
{
    public class PickUp : MonoBehaviour
    {
        [SerializeField] private string _type;

        private void Start()
        {
            Debug.Log("Pick Up type: " + _type);
        }
    }
}
