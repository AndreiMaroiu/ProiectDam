using Core.Events;
using Gameplay.Events;
using UnityEngine;
using static Core.Events.DoorsEvent;

namespace Gameplay.Managers
{
    public class DoorsManager : MonoBehaviour
    {
        [SerializeField] private DoorsEvent _onDoorBlock;
        [SerializeField] private RoomBehaviourEvent _roomBehaviourEvent;

        private void Start()
        {
            _onDoorBlock.OnLock += OnDoorsBlock;
        }

        private void OnDoorsBlock(UnlockCondition condition)
        {
            if (condition is UnlockCondition.None)
            {
                return;
            }

            var behaviour = _roomBehaviourEvent.Value.ActiveLayerBehaviour;

            behaviour.UpdateDoors(true);
            behaviour.OnAllEnemiesKilled += () =>
            {
                behaviour.UpdateDoors(false);
            };
        }

        private void OnDestroy()
        {
            _onDoorBlock.OnLock -= OnDoorsBlock;
        }
    }
}
