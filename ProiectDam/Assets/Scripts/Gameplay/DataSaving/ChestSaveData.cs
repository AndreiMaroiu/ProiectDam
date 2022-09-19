using Gameplay.DataSaving;

namespace Gameplay
{
    [System.Serializable]
    public class ChestSaveData : ObjectSaveData
    {
        public bool WasOpened { get; set; }
    }
}
