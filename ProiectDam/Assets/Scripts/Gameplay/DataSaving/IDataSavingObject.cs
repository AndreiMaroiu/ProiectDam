namespace Gameplay.DataSaving
{
    public interface IDataSavingObject<T>
    {
        public void LoadFromSave(T saveData);
        public T SaveData { get; }
    }
}
