using UnityEngine;

namespace Core.DataSaving
{
    public abstract class LevelSaverManager : MonoBehaviour
    {
        public abstract void Save();
        public abstract void SetUpForNewScene();
        public abstract void SaveOnlySeed();
    }
}
