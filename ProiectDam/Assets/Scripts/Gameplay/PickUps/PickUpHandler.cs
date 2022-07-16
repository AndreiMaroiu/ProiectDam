using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Gameplay.Player;

namespace Gameplay.PickUps
{
    [Serializable]
    public class PickUpHandler
    {
        [SerializeField]
        private string _type;

        public PickUpHandler(string type)
        {
            _type = type;
        }

        public string Type => _type;
    }
}
