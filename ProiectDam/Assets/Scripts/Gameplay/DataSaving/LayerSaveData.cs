using Gameplay.Generation;
using System.Collections.Generic;

namespace Gameplay.DataSaving
{
    [System.Serializable]
    public class LayerSaveData
    {
        public BiomeType Biome { get; set; }
        public TileType[,] Layers { get; set; }

        public Dictionary<Vector2IntPos, ObjectSaveData> DynamicObjects { get; set; } = new Dictionary<Vector2IntPos, ObjectSaveData>();
    }
}
