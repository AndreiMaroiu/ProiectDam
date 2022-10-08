using Gameplay.Generation;
using System.Collections.Generic;

namespace Gameplay.DataSaving
{
    [System.Serializable]
    public sealed class LayersSaveData
    {
        public List<LayerSaveData> Layers { get; set; } = new();
        public bool IsDiscovered { get; set; }

        public LayerSaveData GetFromBiome(BiomeType biome)
        {
            foreach (var layer in Layers)
            {
                if (layer.Biome == biome)
                {
                    return layer;
                }
            }

            return null;
        }
    }
}
