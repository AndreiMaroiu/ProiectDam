using Core;
using Gameplay.Events;
using UnityEngine;
using Utilities;
using Core.Values;

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
        
        private DungeonGenerator _generator;

        public override void Spawn()
        {
            GenerateDungeon();
            GenerateRoomTypes();

            GenerateAndSpawnLevel();
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
                Room room;
                int halfCount = distances.Count / 2;
                int countMinus = distances.Count - 1;

                do
                {
                    int distance = Random.Range(halfCount, countMinus);
                    room = distances[distance].room;
                } while (room.Type != RoomType.Normal);

                Validate(room.Pos, type);
            }
        }
    }
}