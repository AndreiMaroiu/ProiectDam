using UnityEngine;

namespace Core.Events.Binding
{
    public sealed class Binder : MonoBehaviour
    {
        [SerializeField] private ScriptableObject _event;
        [SerializeField] private MonoBehaviour _target;

        private void OnValidate()
        {
            bool isBindSource = _event != null && _event is IBindSource;
            bool isBindTarget = _target != null && _target is IBindTarget;

            if (isBindSource is false)
            {
                Debug.LogError("Event is not implement from IBindSource");
            }

            if (isBindTarget is false)
            {
                Debug.LogError("Target does not implement from IBindTarget");
            }
        }

        private void Start()
        {
            if (_event is IBindSource bindSource)
            {
                bool succes = bindSource.SimpleBindable.Bind(_target as IBindTarget);

                if (succes is false)
                {
                    Debug.LogError("Could not bind source to target!");
                }
            }
        }

        private void OnDestroy()
        {
            if (_event is IBindSource bindSource)
            {
                bool succes = bindSource.SimpleBindable.UnBind(_target as IBindTarget);

                if (succes is false)
                {
                    Debug.LogError("Could not bind source to target!");
                }
            }
        }
    }
}
