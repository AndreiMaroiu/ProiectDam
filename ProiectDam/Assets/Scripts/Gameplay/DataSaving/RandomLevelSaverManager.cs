using Core.DataSaving;
using Gameplay.Generation;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.DataSaving
{
    public class RandomLevelSaverManager : LevelSaverManager
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private RandomLevelSpawner _levelSpawner;
        [SerializeField] private LevelSaverHandler _handler;

        public void Init()
        {
            if (_handler.ShouldLoad)
            {
                Debug.Log("reading from save");
                bool read = Utilities.BinaryReader.TryRead(_handler.SaveFile, out LevelSaveData data);
                if (read)
                {
                    Debug.Log("data read from save!");
                    SaveData = data;

                    // todo: apply all data
                }
                else
                {
                    Debug.LogError("Could not read file: " + _handler.SaveFile);
                }
            }
        }

        public override void Save() // todo: add string file
        {
            // todo: write to file

            Debug.Log("saving level data");

            SaveData = new LevelSaveData()
            {
                Seed = _levelSpawner.Seed,
                PlayerData = new PlayerSaveData()
                {
                    PlayerPos = _player.transform.position,
                    Bullets = _player.Bullets,
                    Health = _player.Health,
                    Energy = _player.Energy,
                    Score = _player.Score,
                    Coins = _player.Money,
                }
            };

            Utilities.BinaryReader.Write(Application.persistentDataPath + "/Save.dat", SaveData);
        }

        public bool ShouldLoad => _handler.ShouldLoad;

        public LevelSaveData SaveData { get; private set; }
    }
}
