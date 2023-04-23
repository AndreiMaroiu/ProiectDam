using Core.Values;

namespace Gameplay.DataSaving
{
    [System.Serializable]
    public struct LayerPositionData
    {
        public Vector2IntPos Position { get; set; }
        public int Biome { get; set; }
    }
}