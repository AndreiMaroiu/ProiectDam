using UnityEngine;

namespace Core.DataSaving
{
    public abstract class LevelSaverHandler : ScriptableObject
    {
        public abstract bool ShouldLoad { get; }
        public abstract string SaveFile { get; }

        /// <summary>
        /// mark level saver to load level from a specified file
        /// </summary>
        /// <param name="saveFile">the name of the save file</param>
        public abstract void Load(string saveFile);
        /// <summary>
        /// mark level saver to not load data for a level
        /// </summary>
        public abstract void SetForNewScene();

    }
}
