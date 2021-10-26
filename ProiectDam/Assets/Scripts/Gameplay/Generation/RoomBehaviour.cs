using Core;
using Gameplay.Enemies;
using UnityEngine;

namespace Gameplay.Generation
{
    public sealed class RoomBehaviour : MonoBehaviour
    {
        private GameObject[] _layersObjects;
        private BaseEnemy[] _enemies;
        private DoorBehaviour[] _doors;

        private int _enemiesCount;
        private Room _room;
        private Layers _layers;
        private int _currentLayer;

        public Room Room => _room;
        public Layers Layers => _layers;
        public int CurrentLayer => _currentLayer;

        private GameObject CreateEmptyObject(string name)
        {
            GameObject result = new GameObject(name);
            result.transform.parent = transform;
            result.transform.localPosition = Vector3.zero;
            return result;
        }

        public void Set(Room room, Layers layers)
        {
            _room = room;
            _layers = layers;
            _layersObjects = new GameObject[layers.Count];

            for(int i = 0; i < layers.Count; i++)
            {
                _layersObjects[i] = CreateEmptyObject($"Layer {i.ToString()}");
                _layersObjects[i].SetActive(false);
            }

            _currentLayer = _layers.Count / 2;
            _layersObjects[_currentLayer].SetActive(true);
        }

        public void ChangedLayer(int layer)
        {
            if (layer >= _layers.Count)
            {
                return;
            }

            _layersObjects[_currentLayer].SetActive(false);
            _currentLayer = layer;
            _layersObjects[layer].SetActive(true);
        }

        public void OnRoomEnter()
        {
            // trigger event for current room behaviour
        }

        public Transform GetTransform(int index) 
            => _layersObjects[index].transform;

        public void Scan()
        {
            ScanForEnemies();
            ScanForDoors();
        }

        private void ScanForDoors()
        {
            _doors = GetComponentsInChildren<DoorBehaviour>();

            UpdateDoors(isLocked: true);
        }

        private void ScanForEnemies()
        {
            _enemies = GetComponentsInChildren<BaseEnemy>();
            _enemiesCount = _enemies.Length;

            foreach (BaseEnemy enemy in _enemies)
            {
                enemy.OnDeathEvent += OnEnemyDeath;
            }
        }

        private void OnEnemyDeath(BaseEnemy enemy)
        {
            --_enemiesCount;

            if (_enemiesCount > 0)
            {
                return;
            }

            UpdateDoors(isLocked: false);
        }

        private void UpdateDoors(bool isLocked)
        {
            foreach (DoorBehaviour door in _doors)
            {
                door.IsLocked = isLocked;
            }
        }
    }
}
