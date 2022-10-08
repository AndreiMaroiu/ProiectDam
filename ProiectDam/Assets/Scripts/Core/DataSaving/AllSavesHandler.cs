using UnityEngine;

namespace Core.DataSaving
{
    public abstract class AllSavesHandler : ScriptableObject
    {
        public abstract SaveSummary[] Summaries { get; }

        public abstract string GetSaveFilePath(int i);

        public abstract bool TrySaveData(object saveData, string path);
    }
}
