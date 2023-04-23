using Core;
using Core.DataSaving;
using Core.Events;
using Core.Values;
using Gameplay.Generation;
using Gameplay.Managers;
using Gameplay.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

namespace Gameplay.DataSaving
{
    public sealed class RandomLevelSaverManager : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private RandomLevelSpawner _levelSpawner;
        [SerializeField] private RandomAllSavesManager _allSaves;
        [SerializeField] private LevelSaverHandler _handler;
        [SerializeField] private RoomEvent _roomEvent;
        [SerializeField] private TurnManager _turnManager;
        [SerializeField] private SaveEvent _saveEvent;
        [SerializeField] private FloatValue _difficultyMultiplier;

        private bool _loaded;
        private LevelSaveData _saveData;

        private void Init()
        {
            lock (this)
            {
                if (_handler.ShouldLoad)
                {
                    string fullPath = _handler.SaveFile.RunPath;
                    LevelSaveData data = _allSaves.GetSave(fullPath);

                    if (data is not null)
                    {
                        SaveData = data;
                    }
                    else
                    {
                        Debug.LogError("Could not read file: " + fullPath);
                    }
                }

                Debug.Log(_handler.ShouldLoad ? "Should Load" : "Should Generate");
            }
        }

        #region Unity Events

        private void Start()
        {
            _saveEvent.OnEvent += OnSave;
        }

        private void OnDestroy()
        {
            _saveEvent.OnEvent -= OnSave;
        }

        #endregion

        private void OnSave(SaveType type)
        {
            switch (type)
            {
                case SaveType.Save:
                    Save();
                    break;
                case SaveType.SaveSeed:
                    SaveOnlySeed();
                    break;
                case SaveType.DontSave:
                    SetUpForNewScene();
                    break;
                case SaveType.UpdateHubInfo:
                    UpdateHubInfo();
                    break;
                default:
                    break;
            }
        }

        private void UpdateHubInfo()
        {
            _ = DataReader.ReadAndSave<HubSaveData>(_handler.SaveFile.SaveDataPath, data => data.Coins += _player.Money);
        }

        public void Save()
        {
            SaveData = new()
            {
                Seed = _levelSpawner.Seed,
                CurrentRoom = _roomEvent.Value.Pos,
                TurnManagerData = _turnManager.SaveData,
                PlayerData = _player.SaveData,
                Rooms = SaveRooms(),
                RunsCount = 1, // TODO: update the runs count
                DifficultyMultiplier = _difficultyMultiplier,
            };

            SavePath savePath = _handler.SaveFile;

            if (savePath.IsNullOrEmpty())
            {
                savePath = _allSaves.GetSaveFilePath(0);
            }

            _allSaves.TrySaveData(SaveData, savePath.RunPath);
            _allSaves.TrySaveData(new SaveSummary()
            {
                Energy = SaveData.PlayerData.Energy.start,
                Health = SaveData.PlayerData.Health.start,
                Money = SaveData.PlayerData.Coins,
                RoomsDiscovered = SaveData.Rooms.Count(x => x.Value.IsDiscovered)
            }, savePath.SummaryPath);

            Debug.Log("Save path: " + savePath);
        }

        public void SetUpForNewScene()
        {
            _handler.SetForNewScene(_handler.SaveFile);
        }

        public void SaveOnlySeed()
        {
            _handler.SetSeed(_levelSpawner.Seed);
        }

        public bool ShouldLoad => _handler.ShouldLoad && SaveData is not null;

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
            Dictionary<Vector2IntPos, LayersSaveData> rooms = new();

            _levelSpawner.Traverser.TraverseUnique(room =>
            {
                RoomBehaviour roomBehaviour = _levelSpawner.Traverser[room.Pos];
                LayersSaveData saveData = GenerateRoomSaveData(roomBehaviour);

                rooms[room.Pos] = saveData;
            });

            return rooms;
        }

        private static LayersSaveData GenerateRoomSaveData(RoomBehaviour roomBehaviour)
        {
            LayersSaveData saveData = new()
            {
                IsDiscovered = roomBehaviour.Room.Discovered
            };

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
