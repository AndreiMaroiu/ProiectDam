using Core.Events;
using UnityEngine;

namespace Gameplay
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private GameEvent _globalDeathEvent;
        [SerializeField] private IntEvent _scoreEvent;

        private void Start()
        {
            _scoreEvent.Value = 0;
            _globalDeathEvent.OnEvent += OnDeath;
        }

        private void OnDestroy()
        {
            _globalDeathEvent.OnEvent -= OnDeath;
        }

        private void OnDeath(object sender)
        {
            if (sender is KillableObject killable)
            {
                _scoreEvent.Value += killable.Score;
            }
        }
    }
}
