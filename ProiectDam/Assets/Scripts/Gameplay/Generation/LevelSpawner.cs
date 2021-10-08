using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay.Generation
{
    internal sealed class LevelSpawner : MonoBehaviour
    {
        [SerializeField] private int _maxRoomNeighbours;
        [SerializeField] private float _distance;
        [Space]
        [SerializeField] private GameObject _room;
        [SerializeField] private GameObject _door;
        [Tooltip("lenght should be odd")]
        [SerializeField] private int _length;
        [SerializeField] private int _maxRoomCount;

        private readonly HashSet<(int x, int y)> _spawnedCoordonates = new HashSet<(int x, int y)>();

        private void Start()
        {
            DungeonGenerator generator = new DungeonGenerator(_maxRoomNeighbours, _maxRoomCount, _length);
            Room start = generator.GenerateDungeon();

            //WriteMatrix(generator.Matrix, "Assets/MatrixFile.txt", append: false);
            //WriteMatrix(generator.Matrix, "Assets/FinalDungeons.txt", append: true);

            GenerateGameAssests(start);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }

        private GameObject SpawnRoom(Room room, Vector3 pos)
        {
            int x = (int)room.Pos.x;
            int y = (int)room.Pos.y;

            var pair = (x, y);

            if (!_spawnedCoordonates.Contains(pair))
            {
                GameObject result = Instantiate(this._room, pos, Quaternion.identity);

                _spawnedCoordonates.Add(pair);

                Color color = room.Type switch
                {
                    RoomType.End => Color.blue,
                    RoomType.Start => Color.green,
                    _ => Color.white,
                };

                result.GetComponent<SpriteRenderer>().color = color;

                return result;
            }

            return null;
        }

        private void SpawnDoor(Room room, Vector3 where)
        {
            if (room.LastRoom is null)
            {
                return;
            }

            Vector3 pos = where - (Utils.GetWorldDirection(room.Direction) * _distance / 2);
            Instantiate(_door, pos, Quaternion.identity);
        }

        private void GenerateGameAssests(Room start)
        {
            Queue<(Room room, Vector3 pos)> queue = new Queue<(Room, Vector3)>();
            GameObject lastRoom = null;

            queue.Enqueue((start, Vector3.zero));

            while (queue.Count > 0)
            {
                var top = queue.Dequeue();
                GameObject temp = SpawnRoom(top.room, top.pos);
                SpawnDoor(top.room, top.pos);

                if (!ReferenceEquals(temp, null))
                {
                    lastRoom = temp;
                }

                foreach (Room room in top.room)
                {
                    queue.Enqueue((room, top.pos + (Utils.GetWorldDirection(room.Direction) * _distance)));
                }
            }

            if (!ReferenceEquals(lastRoom, null))
            {
                lastRoom.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }

        private static void WriteMatrix(RoomType[,] matrix, string path, bool append)
        {
            using StreamWriter file = new StreamWriter(path, append);

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    file.Write(matrix[i, j].FastToString()[0] + " ");
                }

                file.WriteLine();
            }

            file.WriteLine();
        }
    }
}