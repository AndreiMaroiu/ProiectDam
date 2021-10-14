using Gameplay.Enemies;
using UnityEngine;

namespace Gameplay.Generation
{
    public sealed class RoomBehaviour : MonoBehaviour
    {
        private GameObject _baseLayer;
        private GameObject _currentLayer;
        private GameObject _frontLayer;

        private BaseEnemy[] _enemies;
        private DoorBehaviour[] _doors;

        private int _enemiesCount;

        private void Awake()
        {
            _baseLayer = CreateEmptyObject("Base Layer");
            _currentLayer = CreateEmptyObject("Current Layer");
            _frontLayer = CreateEmptyObject("Front Layer");
        }

        private GameObject CreateEmptyObject(string name)
        {
            GameObject result = new GameObject(name);
            result.transform.parent = transform;
            return result;
        }

        public Transform GetTransform(int index) => index switch
        {
            0 => _baseLayer.transform,
            1 => _currentLayer.transform,
            2 => _frontLayer.transform,
            _ => null,
        };

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
