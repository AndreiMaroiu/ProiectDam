using UnityEngine;

namespace Core.DataSaving
{
    public abstract class AllSavesHandler : ScriptableObject
    {
        public abstract SaveSummary GetSummary(int i);

        public abstract SavePath GetSaveFilePath(int i);

        public abstract bool TrySaveData(object saveData, string path);

        public abstract void DeleteSave(int i);

        public abstract bool SaveFilesExist(int i);
    }
}
