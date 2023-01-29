using Gameplay.Enemies;
using Gameplay.Generation;

namespace Gameplay.DataSaving
{
    [System.Serializable]
    public class SimpleEnemySaveData : ObjectSaveData
    {
        public int Health { get; set; }

        public bool IsFlipped { get; set; }

        public TileType LastTile { get; set; }
        public bool CanHit { get; set; }
    }

    [System.Serializable]
    public class BossEnemySaveData : SimpleEnemySaveData
    {
        public BossEnemy.BossPhase Phase { get; set; }
    }
}
