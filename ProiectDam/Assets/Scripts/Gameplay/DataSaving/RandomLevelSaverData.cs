using Core.DataSaving;
using System;
using UnityEngine;

namespace Gameplay.DataSaving
{
    // todo: refactor, maybe move the seed data in another scriptable
    [CreateAssetMenu(fileName = "New Level Saver", menuName = "Scriptables/Level Saver")]
    public sealed class RandomLevelSaverData : LevelSaverHandler
    {
        private const string SAVE_FILE_STR = "GenerationSaveFile";
        private const string SHOULD_LOAD_STR = "ShouldLoad";
        private const string SEED_STR = "GenerationSeed";

        [SerializeField] private bool _shouldLoad;
        [SerializeField] private int _seed;

        private string _saveFile;

        #region Level Saver Handler

        public override bool ShouldLoad => _shouldLoad;

        public override string SaveFile => _saveFile;

        public override int Seed => _seed;

        public override void Load(string saveFile)
        {
            _shouldLoad = true;
            _saveFile = saveFile;
            SaveState();
        }

        public override void SetForNewScene(string saveFile)
        {
            int seed = GenerateRandomSeed();
            Debug.Log("Generated seed: " + seed.ToString());
            SetSeed(seed);
        }

        public override void SetSeed(int seed)
        {
            _seed = seed;

            _shouldLoad = false;
            _saveFile = null;

            SaveState();

            Debug.Log("Seed was set with value: " + seed.ToString());
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
            _seed = PlayerPrefs.GetInt(SEED_STR, GenerateRandomSeed());

            SaveState();
        }

        private void SaveState()
        {
            PlayerPrefs.SetString(SAVE_FILE_STR, SaveFile);
            PlayerPrefs.SetInt(SHOULD_LOAD_STR, ToInt(ShouldLoad));
            PlayerPrefs.SetInt(SEED_STR, Seed);
            PlayerPrefs.Save();
        }

        #endregion

        #region Static Helpers

        private static bool ToBool(int i) => i > 0;

        private static int ToInt(bool b) => b ? 1 : 0;

        private static int GenerateRandomSeed() => (int)DateTimeOffset.Now.ToUnixTimeMilliseconds();

        #endregion
    }
}
