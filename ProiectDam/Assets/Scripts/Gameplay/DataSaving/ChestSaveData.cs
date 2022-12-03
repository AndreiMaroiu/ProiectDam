using Gameplay.DataSaving;

namespace Gameplay.DataSaving
{
    [System.Serializable]
    public sealed class ChestSaveData : ObjectSaveData
    {
        public bool WasOpened { get; set; }
    }
}
