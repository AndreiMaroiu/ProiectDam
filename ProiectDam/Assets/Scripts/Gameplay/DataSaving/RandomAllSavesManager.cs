using Core;
using Core.DataSaving;
using System.Linq;
using UnityEngine;
using Utilities;

namespace Gameplay.DataSaving
{
    [CreateAssetMenu(fileName = "New All Saves Handler", menuName = "Scriptables/All Saves Handler")]
    public sealed class RandomAllSavesManager : AllSavesHandler
    {
        private static readonly string[] _relativePaths = new[] { "/Save1.dat", "/Save2.dat", "/Save3.dat" };

        private string[] _savePaths;
        private LevelSaveData[] _saves;
        private SaveSummary[] _summaries;

        private bool _loaded;

        private void OnEnable()
        {
            _savePaths = GenerateAbsolutePaths(_relativePaths);
        }

        public override SaveSummary[] Summaries
        {
            get
            {
                if (!_loaded || _summaries is null)
                {
                    Init();
                }

                return _summaries;
            }
        }

        public override string GetSaveFilePath(int i)
        {
            if (!_loaded)
            {
                Init();
            }

            return _savePaths[i];
        }

        public LevelSaveData GetSave(string path)
        {
            if (IsSaveValid(path) && BinaryReader.TryRead<LevelSaveData>(path, out var result))
            {
                return result;
            }

            return null;
        }

        public override bool TrySaveData(object saveData, string path)
        {
            if (saveData is LevelSaveData levelSaveData && IsSaveValid(path))
            {
                BinaryReader.Write(path, levelSaveData);
                return true;
            }

            return false;
        }

        private void Init()
        {
            _savePaths ??= GenerateAbsolutePaths(_relativePaths);

            _saves = new LevelSaveData[_relativePaths.Length];
            _summaries = new SaveSummary[_relativePaths.Length];

            for (int i = 0; i < _relativePaths.Length; i++)
            {
                if (BinaryReader.TryRead<LevelSaveData>(_savePaths[i], out var saveData))
                {
                    _saves[i] = saveData;

                    _summaries[i] = new()
                    {
                        Energy = saveData.PlayerData.Energy.X,
                        Health = saveData.PlayerData.Health.X,
                        Money = saveData.PlayerData.Coins,
                        RoomsDiscovered = saveData.Rooms.Count(pair => pair.Value.IsDiscovered),
                    };
                }
            }

            _loaded = true;
        }

        private bool IsSaveValid(string path)
        {
            foreach (var savePath in _savePaths)
            {
                if (savePath == path)
                {
                    return true;
                }
            }

            return false;
        }

        private static string[] GenerateAbsolutePaths(string[] relativePaths)
        {
            string[] result = new string[relativePaths.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Application.persistentDataPath + relativePaths[i];
            }

            return result;
        }
    }
}
