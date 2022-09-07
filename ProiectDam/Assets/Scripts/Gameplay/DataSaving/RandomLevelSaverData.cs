using Core.DataSaving;
using UnityEngine;

namespace Gameplay.DataSaving
{
    [CreateAssetMenu(fileName = "New Level Saver", menuName = "Scriptables/Level Saver")]
    public class RandomLevelSaverData : LevelSaverHandler
    {
        [SerializeField] private bool _shouldLoad;

        private string _saveFile;

        public override bool ShouldLoad => _shouldLoad;

        public override string SaveFile => _saveFile;

        public override void Load(string saveFile)
        {
            _shouldLoad = true;
            _saveFile = saveFile;
        }

        public override void SetForNewScene()
        {
            _shouldLoad = false;
        }

        //private void OnEnable()
        //{
        //    SetForNewScene();
        //}
    }
}
