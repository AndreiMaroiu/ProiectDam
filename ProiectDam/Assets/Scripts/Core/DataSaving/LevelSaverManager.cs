using UnityEngine;

namespace Core.DataSaving
{
    /// <summary>
    /// Manager to handle only data saving
    /// </summary>
    public abstract class LevelSaverManager : MonoBehaviour
    {
        public abstract void Save();
        public abstract void SetUpForNewScene();
        public abstract void SaveOnlySeed();
    }
}
