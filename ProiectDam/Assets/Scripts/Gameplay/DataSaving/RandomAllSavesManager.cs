using Core;
using Core.DataSaving;
using UnityEngine;
using Utilities;

namespace Gameplay.DataSaving
{
    [CreateAssetMenu(fileName = "New All Saves Handler", menuName = "Scriptables/All Saves Handler")]
    public sealed class RandomAllSavesManager : AllSavesHandler
    {
        private static readonly string[] _relativePaths = new[] { "/Save1", "/Save2", "/Save3" };

        public override SaveSummary GetSummary(int i)
        {
            if (DataReader.TryRead<SaveSummary>(GetSaveFilePath(i).SummaryPath, out var saveData))
            {
                return saveData;
            }

            return null;
        }

        public override SavePath GetSaveFilePath(int i)
        {
            return Application.persistentDataPath + _relativePaths[i];
        }

        public LevelSaveData GetSave(string path)
        {
            if (DataReader.TryRead<LevelSaveData>(path, out var result))
            {
                return result;
            }

            return null;
        }

        public override bool TrySaveData(object saveData, string path)
        {
            if (saveData is LevelSaveData levelSaveData)
            {
                DataReader.Write(path, levelSaveData);
                return true;
            }

            if (saveData is SaveSummary summary)
            {
                DataReader.Write(path, summary);
                return true;
            }

            return false;
        }

        public override void DeleteSave(int i)
        {
            SavePath save = GetSaveFilePath(i);

            save.Delete();
        }

        public override bool SaveFilesExist(int i)
        {
            SavePath save = GetSaveFilePath(i);
            return save.Exists();
        }
    }
}
