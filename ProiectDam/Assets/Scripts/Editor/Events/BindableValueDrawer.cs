#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace EditorScripts.Events
{
    public class BindableValueDrawer<TBindable> : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;


            SerializedProperty value = property.FindPropertyRelative("_value");

            EditorGUI.PropertyField(position, value, GUIContent.none);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}

#endif