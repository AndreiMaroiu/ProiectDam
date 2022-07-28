using Gameplay.PickUps;
using System;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace EditorScripts
{
    [CustomPropertyDrawer(typeof(PickUpHandler))]
    public class PickUpHandlerDrawer : PropertyDrawer
    {
        private readonly string[] _choices = new PickUpFactory().Names;
        private int _choiceIndex = 0;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect rect = new Rect(position.x, position.y, position.width, position.height);
            SerializedProperty type = property.FindPropertyRelative("_type");
            _choiceIndex = Mathf.Clamp(Array.IndexOf(_choices, type.stringValue), 0, _choices.Length);

            //int indent = EditorGUI.indentLevel;
            //EditorGUI.indentLevel = 0;

            _choiceIndex = EditorGUI.Popup(rect, "Type", _choiceIndex, _choices);
            type.stringValue = _choices[_choiceIndex];

            //EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}

#endif
