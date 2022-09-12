namespace Gameplay.DataSaving
{
    public interface IDataSavingTile : IDataSavingObject<ObjectSaveData>
    {
        public string ObjectName { get; set; }
    }
}
