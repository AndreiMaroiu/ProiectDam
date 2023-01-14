using Gameplay.DataSaving;
using Gameplay.Enemies;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Generation
{
    public class LayerBehaviour : MonoBehaviour
    {
        private DoorBehaviour[] _doors;

        public List<BaseEnemy> Enemies { get; } = new();
        public event Action OnAllEnemiesKilled;

        public bool HasDoors => _doors is not null || _doors.Length > 0;

        private void Start()
        {
            BaseEnemy[] enemies = gameObject.GetComponentsInChildren<BaseEnemy>();
            Enemies.AddRange(enemies);

            foreach (BaseEnemy enemy in enemies)
            {
                enemy.OnDeathEvent += OnEnemyDeath;
            }
        }

        public void UpdateDoors(bool isLocked)
        {
            if (!HasDoors)
            {
                return;
            }
            
            foreach (DoorBehaviour door in _doors)
            {
                door.IsLocked = isLocked;
            }
        }

        public IDataSavingTile[] GetDynamicObject()
        {
            return GetComponentsInChildren<IDataSavingTile>(includeInactive: true);
        }

        private void OnEnemyDeath(BaseEnemy enemy)
        {
            Enemies.Remove(enemy);

            if (Enemies.Count == 0)
            {
                OnAllEnemiesKilled?.Invoke();
            }
        }

        public void ScanForDoors()
        {
            _doors = GetComponentsInChildren<DoorBehaviour>();

            UpdateDoors(isLocked: false); // doors are open by default
        }

        public void AddEnemy(BaseEnemy enemy)
        {
            Enemies.Add(enemy);
            enemy.OnDeathEvent += OnEnemyDeath;
        }
    }
}
