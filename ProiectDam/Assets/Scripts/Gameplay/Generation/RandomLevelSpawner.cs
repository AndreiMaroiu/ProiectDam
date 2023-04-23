using Core;
using Core.Events;
using Core.Values;
using Gameplay.DataSaving;
using UnityEngine;

namespace Gameplay.Generation
{
    /// <summary>
    /// Main Behaviour to generate the whole level data in a random way
    /// </summary>
    internal sealed class RandomLevelSpawner : BaseLevelSpawner
    {
        [Header("Settings")]
        [Tooltip("lenght should be odd")]
        [SerializeField] private IntValue _maxtrixSize;
        [SerializeField] private int _maxRoomCount;
        [SerializeField] private int _maxRoomNeighbours;
        [Header("Events")]
        [SerializeField] private LayerEvent _layerEvent;
        [Header("Data Saving")]
        [SerializeField] private RandomLevelSaverManager _levelSaver;

        private DungeonGenerator _generator;

        public int Seed { get; private set; }

        public override void Spawn()
        {
            SetSeed();
            SetDifficulty();

            GenerateDungeon();
            GenerateRoomTypes();

            SpawnRoomAssets();
            GenerateLayers();
            SetDoorPositions();
            SpawnOrLoadLayers();
            SpawnDoors();
            ScanRooms();
            SpawnPlayer();
            SetCurrentRoom();

            Random.InitState((int)System.DateTimeOffset.Now.ToUnixTimeMilliseconds());
        }

        private void SetDifficulty()
        {
            _data.DifficultyMultiplier = _levelSaver.ShouldLoad ? 1 + (0.5f * (_levelSaver.SaveData.RunsCount - 1)) : 1;
            Debug.Log("Difficulty: " + _data.DifficultyMultiplier);
        }

        private void SetCurrentRoom()
        {
            _data.RoomBehaviourEvent.Value = _levelSaver.ShouldLoad ? _traverser[_levelSaver.SaveData.CurrentRoom] : _traverser.Start;
        }

        private void SetSeed()
        {
            if (_levelSaver.ShouldLoad && _levelSaver.SaveData != null)
            {
                Seed = _levelSaver.SaveData.Seed;
            }
            else
            {
                Seed = _levelSaver.Seed;
            }

            Debug.Log("seed: " + Seed.ToString());
            Random.InitState(Seed);
        }

        /// <summary>
        /// Generate dungeon room layout (only room positions)
        /// </summary>
        private void GenerateDungeon()
        {
            _generator = new DungeonGenerator(_maxRoomNeighbours, (_maxRoomCount * _data.DifficultyMultiplier).RoundToInt(), _maxtrixSize);

            do
            {
                Room start = _generator.GenerateDungeon();
                _traverser = new RoomTraverser<RoomBehaviour>(start);
            } while (Mathf.Abs(_generator.NumberOfRooms - _maxRoomCount) > 3);
        }

        private void SpawnOrLoadLayers()
        {
            if (!_levelSaver.ShouldLoad) // generate layers
            {
                SpawnLayers();
                Debug.Log("Spawn from generated!");
            }
            else // load layers
            {
                SpawnLayersFromSave();
                Debug.Log("Spawn from load");
            }
        }

        private void SpawnLayersFromSave()
        {
            LayersSpawner spawner = new(_data.CellSize, _data.GrassTiles, _data.FireTiles, _data.DungeonTiles);

            _traverser.TraverseUnique(room =>
            {
                _data.Reset();

                RoomBehaviour behaviour = _traverser[room.Pos];
                LayersSaveData saveData = _levelSaver.SaveData.Rooms[room.Pos];
                spawner.SaveData = saveData;

                spawner.SpawnStatic(behaviour);
                behaviour.Layers.SetFromSave(saveData);
                spawner.SetGenerationStrategy(LayersSpawner.GenerationStrategy.FromSave);
                spawner.SpawnDynamic(behaviour);

                room.Discovered = saveData.IsDiscovered;
            });
        }

        /// <summary>
        /// Choose random some room types such as heal, chest etc.
        /// </summary>
        private void GenerateRoomTypes()
        {
            _generator.CalculateDistances();
            var distances = _generator.Distances;

            distances[0].room.Type = RoomType.Start;

            int totalUniqueRoomsCount = 2; // two unique rooms by default: start and end

            ChooseRoomRandom(RoomType.Healing);
            ChooseRoomRandom(RoomType.Chest);
            ChooseRoomRandom(RoomType.Merchant);

            void ChooseRoomRandom(RoomType type)
            {
                if (distances.Count <= totalUniqueRoomsCount) // cannot choose a new room
                {
                    return;
                }

                Room room;
                int halfCount = distances.Count / 2;
                int countMinus = distances.Count - 1;

                do
                {
                    int distance = Random.Range(halfCount, countMinus);
                    room = distances[distance].room;
                    halfCount /= 2;
                } while (room.Type != RoomType.Normal);

                ++totalUniqueRoomsCount;
            }
        }

        protected override void SpawnPlayer()
        {
            if (_levelSaver.ShouldLoad)
            {
                RoomBehaviour behaviour = _traverser[_levelSaver.SaveData.CurrentRoom];
                int layerIndex = _levelSaver.SaveData.PlayerData.LayerPos.Biome;
                TileType[,] layer = behaviour.Layers[layerIndex];
                var pos = _levelSaver.SaveData.PlayerData.LayerPos.Position;
                layer[pos.X, pos.Y] = TileType.Player; // todo: this may not be needed in future
                _player.LayerPosition = new(pos, layer);
                _player.transform.position = _levelSaver.SaveData.PlayerData.PlayerPos;

                _layerEvent.CurrentLayer.Value = layerIndex;
                behaviour.ChangedLayer(layerIndex);
                _layerEvent.CurrentBiome.Value = behaviour.Layers.GetBiome(layerIndex);
            }
            else
            {
                base.SpawnPlayer();
            }
        }
    }
}