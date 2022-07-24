using Core.Events;
using Core.Values;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "New Enemy Data", menuName = "Scriptables/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private int _damage;
        [SerializeField] private int _startHealth = 1;
        [SerializeField] private float _moveTime;
        [SerializeField] private FloatValue _cellSizeValue;
        [SerializeField] private GameEvent _globalDeathEvent;

        public int Damage => _damage;
        public int StartHealth => _startHealth;
        public float MoveTime => _moveTime;
        public FloatValue CellSizeValue => _cellSizeValue;
        public GameEvent GlobalDeathEvent => _globalDeathEvent;
    }
}
