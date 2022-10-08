using Gameplay.Generation;

namespace Gameplay.DataSaving
{
    [System.Serializable]
    public sealed class SimpleEnemySaveData : ObjectSaveData
    {
        public int Health { get; set; }

        public bool IsFlipped { get; set; }

        public TileType LastTile { get; set; }
    }
}
