using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "New Player Stats", menuName = "Scriptables/Player Stats")]
    public class PlayerStats : ScriptableObject
    {
        [SerializeField] private int _bulletCount;
        [SerializeField] private int _energyPerMove;
        [SerializeField] private int _energyPerAttack;

        public int BulletCount => _bulletCount;
        public int EnergyPerMove => _energyPerMove;
        public int EnergyPerAttack => _energyPerAttack;
    }
}
