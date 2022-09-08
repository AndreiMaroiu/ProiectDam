using Gameplay.DataSaving;
using Gameplay.Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class LayerBehaviour : MonoBehaviour
    {
        public List<BaseEnemy> Enemies { get; } = new List<BaseEnemy>();

        private void Start()
        {
            BaseEnemy[] enemies = gameObject.GetComponentsInChildren<BaseEnemy>();
            Enemies.AddRange(enemies);

            foreach (BaseEnemy enemy in enemies)
            {
                enemy.OnDeathEvent += OnEnemyDeath;
            }
        }

        private void OnEnemyDeath(BaseEnemy enemy)
        {
            Enemies.Remove(enemy);
        }

        public IDataSavingObject[] GetDynamicObject()
        {
            return GetComponentsInChildren<IDataSavingObject>();
        }
    }
}
