namespace Gameplay.DataSaving
{

    public interface IDataSavingObject
    {
        public string ObjectName { get; set; }
        public ObjectSaveData SaveData { get; }
        public void LoadFromSave(ObjectSaveData data);
    }
}
