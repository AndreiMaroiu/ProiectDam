using System;

namespace Gameplay.DataSaving
{
    public interface IDataSavingTile : IDataSavingObject<ObjectSaveData>
    {
        public Guid ObjectId { get; set; }
    }
}
