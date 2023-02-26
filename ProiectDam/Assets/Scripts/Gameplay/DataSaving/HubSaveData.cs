using System;
using System.Collections.Generic;

namespace Gameplay.DataSaving
{
    [Serializable]
    internal class HubSaveData
    {
        public int Coins { get; set; }

        public List<PersistentPickUpSaveData> SingleTimePickUps { get; set; } = new();
    }
}
