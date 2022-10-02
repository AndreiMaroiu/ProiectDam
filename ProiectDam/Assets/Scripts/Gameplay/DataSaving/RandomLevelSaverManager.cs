using Core.DataSaving;
using Core.Events;
using Gameplay.Generation;
using Gameplay.Managers;
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
        [SerializeField] private TurnManager _turnManager;

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
                TurnManagerData = _turnManager.SaveData,
                PlayerData = _player.SaveData,
                Rooms = SaveRooms(),
            };

            Utilities.BinaryReader.Write(Application.persistentDataPath + "/Save.dat", SaveData);
        }

        public override void SetUpForNewScene()
        {
            _handler.SetForNewScene();
        }

        public override void SaveOnlySeed()
        {
            _handler.SetSeed(_levelSpawner.Seed);
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
                if (!_loaded && _handler.ShouldLoad)
                {
                    Init();
                }

                return _saveData;
            }
            private set
            {
                _saveData = value;
                _loaded = true;
            }
        }

        private Dictionary<Vector2IntPos, LayersSaveData> SaveRooms()
        {
            Dictionary<Vector2IntPos, LayersSaveData> rooms = new Dictionary<Vector2IntPos, LayersSaveData>();

            _levelSpawner.Traverser.Traverse(room =>
            {
                RoomBehaviour roomBehaviour = _levelSpawner.Traverser[room.Pos];
                LayersSaveData saveData = GenerateRoomSaveData(roomBehaviour);

                rooms[room.Pos] = saveData;
            });

            return rooms;
        }

        private static LayersSaveData GenerateRoomSaveData(RoomBehaviour roomBehaviour)
        {
            LayersSaveData saveData = new LayersSaveData();

            for (int i = 0; i < roomBehaviour.Layers.Count; i++)
            {
                LayerSaveData layerSaveData = GenerateLayerSaveData(roomBehaviour, i);

                saveData.Layers.Add(layerSaveData);
            }

            return saveData;
        }

        private static LayerSaveData GenerateLayerSaveData(RoomBehaviour roomBehaviour, int i)
        {
            LayerSaveData layerSaveData = new()
            {
                Layers = roomBehaviour.Layers.GetTiles(i),
                Biome = roomBehaviour.Layers.GetBiome(i),
            };

            IDataSavingTile[] dynamics = roomBehaviour.LayerBehaviours[i].GetDynamicObject();

            foreach (var dynamic in dynamics)
            {
                if (dynamic is TileObject tileObject)
                {
                    Vector2IntPos pos = tileObject.LayerPosition.Position;

#if UNITY_EDITOR
                    if (layerSaveData.DynamicObjects.ContainsKey(pos))
                    {
                        Debug.LogError($"Key already defined in dynamic objects for layer {i.ToString()} in room with pos: {roomBehaviour.Room.Pos.ToString()}");
                    }
#endif

                    layerSaveData.DynamicObjects[pos] = dynamic.SaveData;
                }
            }

            return layerSaveData;
        }
    }
}
