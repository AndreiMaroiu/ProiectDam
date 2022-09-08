using Gameplay.Generation;
using System.Collections.Generic;

namespace Gameplay.DataSaving
{
    [System.Serializable]
    public class LayersSaveData
    {
        public List<LayerSaveData> Layers { get; set; } = new List<LayerSaveData>();

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
