using System;

namespace Gameplay.DataSaving
{
    [System.Serializable]
    public abstract class ObjectSaveData
    {
        public Guid ObjectId { get; set; }
    }
}
