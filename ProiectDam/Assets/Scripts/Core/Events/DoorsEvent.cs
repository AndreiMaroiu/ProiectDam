using System;
using UnityEngine;
using Core.Events.Binding;

namespace Core.Events
{
    [CreateAssetMenu(fileName = "New Doors Event", menuName = "Scriptables/Events/Doors Event")]
    public sealed class DoorsEvent : ScriptableObject, IBindSource<DoorsEvent.UnlockCondition>
    {
        private readonly BindableEvent<UnlockCondition> _bindable = new();

        public IBindable<UnlockCondition> Bindable => _bindable;

        IBindable IBindSource.SimpleBindable => _bindable;

        public event Action<UnlockCondition> OnLock
        {
            add => _bindable.OnValueChanged += value;
            remove => _bindable.OnValueChanged -= value;
        }

        public void LockDoors(UnlockCondition unlockCondition) => _bindable.Invoke(unlockCondition);

        public enum UnlockCondition
        {
            None, // this will not block doors, as for now
            AllEnemiesKilled,
        }
    }
}
