#if UNITY_EDITOR

using Core.Events;
using Core.Events.Binding;
using UnityEditor;
using UnityEngine;

namespace EditorScripts.Events
{
    [CustomEditor(typeof(CappedIntEvent)), CanEditMultipleObjects]
    public class CappedIntEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying is false || target == null)
            {
                return;
            }

            if (GUILayout.Button("Apply change"))
            {
                CappedIntEvent @event = (CappedIntEvent)target;

                (@event.MaxValueBindable as BindableValue<int>).Invoke();
                (@event.ValueBindable as BindableValue<int>).Invoke();
            }
        }
    }
}

#endif
