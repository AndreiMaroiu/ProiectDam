using Core.DataSaving;
using UnityEngine;

namespace Gameplay.DataSaving
{
    // todo: refactor into static class
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
            ForceSerialization();
        }

        public override void SetForNewScene()
        {
            _shouldLoad = false;
            ForceSerialization();
        }

        private void ForceSerialization()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        //private void OnEnable()
        //{
        //    SetForNewScene();
        //}
    }
}
