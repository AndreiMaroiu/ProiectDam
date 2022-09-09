using Core.DataSaving;
using System;
using UnityEngine;

namespace Gameplay.DataSaving
{
    // todo: refactor, maybe move the seed data in another scriptable
    [CreateAssetMenu(fileName = "New Level Saver", menuName = "Scriptables/Level Saver")]
    public class RandomLevelSaverData : LevelSaverHandler
    {
        private const string SAVE_FILE_STR = "GenerationSaveFile";
        private const string SHOULD_LOAD_STR = "ShouldLoad";
        private const string SEED_STR = "GenerationSeed";

        [SerializeField] private bool _shouldLoad;

        private string _saveFile;
        private int? _seed;

        #region Level Saver Handler

        public override bool ShouldLoad => _shouldLoad;

        public override string SaveFile => _saveFile;

        public override int Seed => _seed.Value;

        public override void Load(string saveFile)
        {
            _shouldLoad = true;
            _saveFile = saveFile;
            SaveState();
        }

        public override void SetForNewScene()
        {
            _shouldLoad = false;
            _seed = GenerateRandomSeed;
            SaveState();
        }

        public override void SetSeed(int seed)
        {
            _seed = seed;

            _shouldLoad = false;
            _saveFile = null;

            SaveState();
        }

        #endregion

        #region Unity Events

        public void OnEnable()
        {
            LoadState();
        }

        public void OnDisable()
        {
            SaveState();
        }

        #endregion

        #region Private Methods

        private void LoadState()
        {
            _saveFile = PlayerPrefs.GetString(SAVE_FILE_STR, defaultValue: null);
            _shouldLoad = ToBool(PlayerPrefs.GetInt(SHOULD_LOAD_STR, defaultValue: 0));

            if (PlayerPrefs.HasKey(SEED_STR))
            {
                _seed = PlayerPrefs.GetInt(SEED_STR);
            }
            else
            {
                _seed = GenerateRandomSeed;
            }
        }

        private void SaveState()
        {
            PlayerPrefs.SetString(SAVE_FILE_STR, SaveFile);
            PlayerPrefs.SetInt(SHOULD_LOAD_STR, ToInt(ShouldLoad));

            if (_seed is null)
            {
                PlayerPrefs.DeleteKey(SEED_STR);
            }
            else
            {
                PlayerPrefs.SetInt(SEED_STR, Seed);
            }
        }

        #endregion

        #region Static Helpers

        private static bool ToBool(int i) => i > 0;

        private static int ToInt(bool b) => b ? 1 : 0;

        private static int GenerateRandomSeed => (int)DateTimeOffset.Now.ToUnixTimeMilliseconds();

        #endregion
    }
}
