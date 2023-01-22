using Core.DataSaving;
using System;
using UnityEngine;

namespace Gameplay.DataSaving
{
    [CreateAssetMenu(fileName = "New Level Saver", menuName = "Scriptables/Level Saver")]
    public sealed class RandomLevelSaverData : LevelSaverHandler
    {
        private static bool _shouldLoad;
        private static int _seed;
        private static string _saveFile;

        #region Level Saver Handler

        public override bool ShouldLoad => _shouldLoad;

        public override string SaveFile => _saveFile;

        public override int Seed => _seed;

        public override void Load(string saveFile)
        {
            _shouldLoad = true;
            _saveFile = saveFile;
        }

        public override void SetForNewScene(string saveFile)
        {
            _saveFile = saveFile;
            int seed = GenerateRandomSeed();
            Debug.Log("Generated seed: " + seed.ToString());
            SetSeed(seed);
        }

        public override void SetSeed(int seed)
        {
            _seed = seed;
            _shouldLoad = false;

            Debug.Log("Seed was set with value: " + seed.ToString());
        }

        #endregion

        

        private static int GenerateRandomSeed() => (int)DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}
