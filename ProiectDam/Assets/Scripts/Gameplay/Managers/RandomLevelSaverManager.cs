using Core.DataSaving;
using Gameplay.Generation;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Managers
{
    public class RandomLevelSaverManager : LevelSaverManager
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private RandomLevelSpawner _levelSpawner;
        [SerializeField] private LevelSaverData _data;


        private void Start()
        {

        }

        public override void Save()
        {
            _data.Save(this);
        }
    }
}
