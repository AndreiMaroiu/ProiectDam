using UnityEngine;

namespace Core.DataSaving
{
    /// <summary>
    /// Handler for a save manager for data transportation between scenes
    /// </summary>
    public abstract class LevelSaverHandler : ScriptableObject
    {
        public abstract SavePath SaveFile { get; }
        public abstract bool ShouldLoad { get; }
        public abstract int Seed { get; }

        /// <summary>
        /// mark level saver to load level from a specified file
        /// </summary>
        /// <param name="saveFile">the name of the save file</param>
        public abstract void Load(SavePath saveFile);
        /// <summary>
        /// mark level saver to not load data for a level
        /// </summary>
        public abstract void SetForNewScene(SavePath saveFile);

        /// <summary>
        /// Set seed if you want to generate level random from seed, but not fully load all save data
        /// </summary>
        public abstract void SetSeed(int seed);

    }
}
