using Core.Events;
using UnityEngine;

namespace Core.Quests
{
    public sealed class QuestEvents : MonoBehaviour
    {
        [SerializeField] private CappedIntEvent _playerHealth;
        [SerializeField] private CappedIntEvent _playerEnergy;
        [SerializeField] private CappedIntEvent _playerBullets;
        [SerializeField] private IntEvent _playerScore;
        [SerializeField] private IntEvent _playerMoney;
        [SerializeField] private GameEvent _globalDeathEvent;
        [SerializeField] private LayerEvent _layerEvent;
        [SerializeField] private RoomEvent _roomEvent;

        public CappedIntEvent PlayerHealth => _playerHealth;
        public CappedIntEvent PlayerEnergy => _playerEnergy;
        public CappedIntEvent PlayerBullets => _playerBullets;
        public IntEvent PlayerScore => _playerScore;
        public IntEvent PlayerMoney => _playerMoney;
        public GameEvent GlobalDeathEvent => _globalDeathEvent;
        public LayerEvent LayerEvent => _layerEvent;
        public RoomEvent RoomEvent => _roomEvent;
    }
}
