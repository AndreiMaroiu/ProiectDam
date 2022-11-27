using UnityEngine;

namespace Core.Events
{
    public class Binder : MonoBehaviour
    {
        [SerializeField] private ScriptableObject _event;
        [SerializeField] private MonoBehaviour _target;

        private void OnValidate()
        {
            //bool isBindSource = _event is not null and IBindSource;
            //bool isBindTarget = _target is not null and IBindTarget;

            //if (isBindSource is false)
            //{
            //    Debug.LogError("Event is not implement from IBindSource");
            //}

            //if (isBindTarget is false)
            //{
            //    Debug.LogError("Target does not implement from IBindTarget");
            //}
        }

        private void Start()
        {
            if (_event is IBindSource bindSource)
            {
                bool succes = bindSource.Bindable.Bind(_target as IBindTarget);

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
                bool succes = bindSource.Bindable.UnBind(_target as IBindTarget);

                if (succes is false)
                {
                    Debug.LogError("Could not bind source to target!");
                }
            }
        }
    }
}
