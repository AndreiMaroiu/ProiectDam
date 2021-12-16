using Events;
using Gameplay.Enemies;
using Gameplay.Events;
using Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Managers
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [Header("Events")]
        [SerializeField] private RoomBehaviourEvent _currentBehaviour;
        [SerializeField] private BoolEvent _playerTurn;

        private bool _wasRoomChanged = false;

        private void Awake()
        {
            _playerTurn.Value = true;

            _currentBehaviour.OnValueChanged += OnRoomChanged;
            _playerTurn.OnValueChanged += OnPlayerMoveEnd;
        }

        private void OnDestroy()
        {
            _currentBehaviour.OnValueChanged -= OnRoomChanged;
            _playerTurn.OnValueChanged -= OnPlayerMoveEnd;
        }

        private void OnRoomChanged()
        {
            _wasRoomChanged = true;
        }

        private void OnPlayerMoveEnd()
        {
            if (!_playerTurn)
            {
                StartCoroutine(ProcessEnemies());
            }
        }

        private IEnumerator ProcessEnemies()
        {
            if (_wasRoomChanged)
            {
                _wasRoomChanged = false;
                _playerTurn.Value = true;
                yield break;
            }

            List<BaseEnemy> enemies = _currentBehaviour.Value.ActiveLayerBehaviour.Enemies;

            for (int i = 0; i < enemies.Count; i++)
            {
                BaseEnemy enemy = enemies[i];
                enemy.OnEnemyTurn(_player);
                yield return new WaitUntil(() => enemy.IsDead || !enemy.IsMoving);
            }

            _playerTurn.Value = true;
        }
    }
}
