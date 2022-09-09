using Core;
using Core.DataSaving;
using Core.Events;
using Gameplay.Generation;
using Gameplay.Player;
using System.Collections.Generic;
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
            SaveData = new LevelSaveData()
            {
                Seed = _levelSpawner.Seed,
                CurrentRoom = _roomEvent.Value.Pos,
                Rooms = SaveRooms(),
                PlayerData = new PlayerSaveData()
                {
                    PlayerPos = _player.transform.position,
                    Bullets = _player.Bullets,
                    Health = _player.Health,
                    Energy = _player.Energy,
                    Score = _player.Score,
                    Coins = _player.Money,
                    IsFliped = _player.IsFlipped,
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
        public int Seed
        {
            get => _handler.Seed;
            set => _handler.SetSeed(value);
        }

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

        private Dictionary<Vector2IntPos, LayersSaveData> SaveRooms()
        {
            Dictionary<Vector2IntPos, LayersSaveData> rooms = new Dictionary<Vector2IntPos, LayersSaveData>();

            // todo: refactor
            _levelSpawner.Traverser.Traverse(room =>
            {
                LayersSaveData saveData = new LayersSaveData();
                RoomBehaviour roomBehaviour = _levelSpawner.Traverser[room.Pos];

                for (int i = 0; i < roomBehaviour.Layers.Count; i++)
                {
                    LayerSaveData layerSaveData = new LayerSaveData()
                    {
                        Layers = roomBehaviour.Layers.GetTiles(i),
                        Biome = roomBehaviour.Layers.GetBiome(i),
                    };

                    IDataSavingObject[] dynamics = roomBehaviour.LayerBehaviours[i].GetDynamicObject();

                    foreach (var dynamic in dynamics)
                    {
                        if (dynamic is TileObject tileObject)
                        {
                            Vector2IntPos pos = tileObject.LayerPosition.Position;

                            #if UNITY_EDITOR
                            if (layerSaveData.DynamicObjects.ContainsKey(pos))
                            {
                                Debug.LogError("key already defined in dynamic objects");
                            }
                            #endif

                            layerSaveData.DynamicObjects[pos] = dynamic.SaveData;
                        }
                    }

                    saveData.Layers.Add(layerSaveData);
                }

                rooms[room.Pos] = saveData;
            });

            return rooms;
        }
    }
}
