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

            foreach (BaseEnemy enemy in enemies)
            {
                Enemies.Add(enemy);
                enemy.OnDeathEvent += OnEnemyDeath;
            }
        }

        private void OnEnemyDeath(BaseEnemy enemy)
        {
            Enemies.Remove(enemy);
        }
    }
}
