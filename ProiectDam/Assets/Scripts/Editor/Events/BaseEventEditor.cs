using Core.Events;
using Core.Events.Binding;
using UnityEditor;
using UnityEngine;

namespace EditorScripts.Events
{
    [CanEditMultipleObjects]
    public class BaseEventEditor<TEvent, T> : Editor where TEvent : BaseValueEvent<T>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying is false)
            {
                return;
            }

            if (GUILayout.Button("Apply change"))
            {
                TEvent @event = (TEvent)target;

                (@event.Bindable as BindableValue<T>).Invoke();
            }
        }
    }
}
