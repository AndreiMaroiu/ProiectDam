using System;
using UnityEngine;

namespace Core.Events
{
    [CreateAssetMenu(fileName = "New Doors Event", menuName = "Scriptables/Doors Event")]
    public class DoorsEvent : ScriptableObject
    {
        public event Action<UnlockCondition> OnLock;

        public void LockDoors(UnlockCondition unlockCondition)
        {
            OnLock?.Invoke(unlockCondition);
        }
    }

    public enum UnlockCondition
    {
        None, // this will not block doors, as for now
        AllEnemiesKilled,
    }
}
