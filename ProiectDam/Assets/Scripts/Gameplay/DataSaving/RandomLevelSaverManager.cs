using Core.DataSaving;
using Core.Events;
using Gameplay.Events;
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
        [SerializeField] private IntEvent _currentLayerEvent;
        [SerializeField] private RoomEvent _roomEvent;

        private bool _loaded;
        private LevelSaveData _saveData;

        private void Init()
        {
            if (_handler.ShouldLoad)
            {
                bool read = Utilities.BinaryReader.TryRead(_handler.SaveFile, out LevelSaveData data);
                if (read)
                {
                    SaveData = data;
                    _loaded = true;
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
                CurrentRoom = _roomEvent.Value.Pos,
                PlayerData = new PlayerSaveData()
                {
                    PlayerPos = _player.transform.position,
                    Bullets = _player.Bullets,
                    Health = _player.Health,
                    Energy = _player.Energy,
                    Score = _player.Score,
                    Coins = _player.Money,
                    LayerPos = new LayerPositionData()
                    {
                        Biome = _currentLayerEvent.Value,
                        Position = _player.LayerPosition.Position,
                    }
                }
            };

            Utilities.BinaryReader.Write(Application.persistentDataPath + "/Save.dat", SaveData);
        }

        public bool ShouldLoad => _handler.ShouldLoad;

        public LevelSaveData SaveData
        {
            get
            {
                if (!_loaded)
                {
                    Init();
                }

                return _saveData;
            }

            private set
            {
                _loaded = true;
                _saveData = value;
            }
            }
        }
}
