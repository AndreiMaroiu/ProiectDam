using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.DataSaving;

namespace Gameplay.Managers
{
    [CreateAssetMenu(fileName = "New Level Saver", menuName = "Scriptables/Level Saver")]
    public class RandomLevelSaverData : LevelSaverData
    {
        [SerializeField] private int _seed;
        [SerializeField] private bool _shouldLoad;

        public override int Seed => _seed;
        public override bool ShouldLoad => _shouldLoad;

        public override void Save(LevelSaverManager levelSaverManager)
        {

        }

        public void Load(int seed)
        {
            _seed = seed;
            _shouldLoad = true;
        }
    }
}
