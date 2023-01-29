using Core.DataSaving;
using Core.Events;
using Core.Values;
using Gameplay.Generation;
using Gameplay.Managers;
using Gameplay.Player;
using System.Collections.Generic;
using UnityEngine;

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
                    var data = _allSaves.GetSave(_handler.SaveFile);

                    if (data is not null)
                    {
                        SaveData = data;
                    }
                    else
                    {
                        Debug.LogError("Could not read file: " + _handler.SaveFile);
                    }
                }
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
                default:
                    break;
            }
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

            string savePath = _handler.SaveFile;

            if (string.IsNullOrWhiteSpace(savePath)) 
            {
                savePath = _allSaves.GetSaveFilePath(0);
            }

            _allSaves.TrySaveData(SaveData, savePath);

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
