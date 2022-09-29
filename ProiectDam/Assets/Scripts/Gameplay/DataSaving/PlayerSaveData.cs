using System.Collections.Generic;

namespace Gameplay.DataSaving
{
    [System.Serializable]
    public class PlayerSaveData
    {
        public Vector3Pos PlayerPos { get; set; }
        public LayerPositionData LayerPos { get; set; }
        public bool IsFliped { get; set; }

        public Vector2IntPos Health { get; set; }
        public Vector2IntPos Energy { get; set; }
        public Vector2IntPos Bullets { get; set; }
        public int Coins { get; set; }
        public int Score { get; set; }
        public List<PersistentPickUpSaveData> PersistentPickUps { get; set; }
    }
}
