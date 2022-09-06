using UnityEngine;

namespace Core.DataSaving
{
    
    public abstract class LevelSaverData : ScriptableObject
    {
        public abstract int Seed { get; }
        public abstract bool ShouldLoad { get; }

        public abstract void Save(LevelSaverManager levelSaverManager);
    }
}
