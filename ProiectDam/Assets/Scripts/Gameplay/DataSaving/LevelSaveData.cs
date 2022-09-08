using System.Collections.Generic;

namespace Gameplay.DataSaving
{
    [System.Serializable]
    public class LevelSaveData
    {
        public int Seed { get; set; }
        public Vector2IntPos CurrentRoom { get; set; }
        public PlayerSaveData PlayerData { get; set; }

        public Dictionary<Vector2IntPos, LayersSaveData> Rooms { get; set; }
    }
}
