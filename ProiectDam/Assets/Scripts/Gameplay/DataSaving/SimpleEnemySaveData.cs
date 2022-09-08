using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.DataSaving
{
    [System.Serializable]
    public class SimpleEnemySaveData : ObjectSaveData
    {
        public int Health { get; set; }

        public bool IsFlipped { get; set; }
    }
}
