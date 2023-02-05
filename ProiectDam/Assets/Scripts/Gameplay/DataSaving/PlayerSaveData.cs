using Core.Values;
using System.Collections.Generic;

namespace Gameplay.DataSaving
{
    [System.Serializable]
    public sealed class PlayerSaveData
    {
        public Vector3Pos PlayerPos { get; set; }
        public LayerPositionData LayerPos { get; set; }
        public bool IsFliped { get; set; }

        public IntRange Health { get; set; }
        public IntRange Energy { get; set; }
        public IntRange Bullets { get; set; }
        public int Coins { get; set; }
        public int Score { get; set; }
        public List<PersistentPickUpSaveData> PersistentPickUps { get; set; }
    }
}
