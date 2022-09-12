using Core.Events;
using Gameplay.DataSaving;
using Gameplay.Enemies;
using Gameplay.Events;
using Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Managers
{
    public class TurnManager : MonoBehaviour, IDataSavingObject<TurnManagerSaveData>
    {
        [SerializeField] private PlayerController _player;
        [Header("Events")]
        [SerializeField] private RoomBehaviourEvent _currentBehaviour;
        [SerializeField] private BoolEvent _playerTurn;

        private bool _wasRoomChanged = false;

        #region Unity Events

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

        #endregion

        #region IDataSaving

        public string ObjectName { get; set; }

        public TurnManagerSaveData SaveData => new TurnManagerSaveData()
        {
            WasRoomChanged = _wasRoomChanged
        };

        public void LoadFromSave(TurnManagerSaveData data)
        {
            _wasRoomChanged = data.WasRoomChanged;
        }

        #endregion

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
                yield return enemy.OnEnemyTurn(_player);
                //enemy.OnEnemyTurn(_player);
                //yield return new WaitUntil(() => enemy.IsDead || !enemy.IsMoving);
            }

            _playerTurn.Value = true;
        }
    }
}
