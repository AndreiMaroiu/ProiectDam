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
        [SerializeField] private FloatValue _difficulty;

        public int Damage => (int)Mathf.Round(_damage * _difficulty);
        public int StartHealth => (int)Mathf.Round(_startHealth * _difficulty);
        public float MoveTime => _moveTime;
        public FloatValue CellSizeValue => _cellSizeValue;
        public GameEvent GlobalDeathEvent => _globalDeathEvent;
    }
}
