using Core;
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
        [Header("Data Saving")]
        [SerializeField] private RandomLevelSaverManager _levelSaver;

        private DungeonGenerator _generator;

        public int Seed { get; private set; }

        public override void Spawn()
        {
            SetSeed();

            GenerateDungeon();
            GenerateRoomTypes();

            SpawnRoomAssets();
            GenereteLayers();
            SetDoorPositions();
            SpawnOrLoadLayers();
            SpawnDoors();

            _data.RoomBehaviourEvent.Value = _traverser.Start;
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
            _generator = new DungeonGenerator(_maxRoomNeighbours, _maxRoomCount, _maxtrixSize);
            Room start = _generator.GenerateDungeon();
            _traverser = new RoomTraverser<RoomBehaviour>(start);
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
            LayersSpawner spawner = new LayersSpawner(_data.CellSize, _data.GrassTiles, _data.FireTiles, _data.DungeonTiles);

            _traverser.TraverseUnique(room =>
            {
                RoomBehaviour behaviour = _traverser[room.Pos];
                LayersSaveData saveData = _levelSaver.SaveData.Rooms[room.Pos];
                spawner.SaveData = saveData;

                spawner.SpawnStatic(behaviour);
                behaviour.Layers.SetFromSave(saveData);
                spawner.SetGenerationStrategy(LayersSpawner.GenerationStrategy.FromSave);
                spawner.SpawnDynamic(behaviour);
            });
        }

        /// <summary>
        /// Choose random some room types such as heal, chest etc.
        /// </summary>
        private void GenerateRoomTypes()
        {
            _generator.CalculateDistances();
            var duplicates = _generator.CalculateDuplicates();
            var distances = _generator.Distances;
            Validate(distances[distances.Count - 1].room.Pos, RoomType.End);

            distances[0].room.Type = RoomType.Start;

            int totalUniqueRoomsCount = 2; // two unique rooms by default: start and end

            ChooseRoomRandom(RoomType.Healing);
            ChooseRoomRandom(RoomType.Chest);
            ChooseRoomRandom(RoomType.Merchant);

            void Validate(Vector2Int pos, RoomType type)
            {
                foreach (Room room in duplicates[pos])
                {
                    room.Type = type;
                }
            }

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
                } while (room.Type != RoomType.Normal);

                Validate(room.Pos, type);

                ++totalUniqueRoomsCount;
            }
        }
    }
}