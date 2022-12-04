using Core.Events;
using Core.Events.Binding;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorScripts
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
                
                (@event.ValueBindable as BindableValue<int>).Invoke();
                (@event.MaxValueBindable as BindableValue<int>).Invoke();
            }
        }
    }
}
