using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Gameplay.DataSaving
{
    [System.Serializable]
    public class PersistentPickUpSaveData
    {
 	    public string Name { get; set; }
        public int Boost { get; set; }
    }
}
