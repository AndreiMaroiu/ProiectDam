using Gameplay.DataSaving;

namespace Gameplay
{
    [System.Serializable]
    public sealed class ChestSaveData : ObjectSaveData
    {
        public bool WasOpened { get; set; }
    }
}
