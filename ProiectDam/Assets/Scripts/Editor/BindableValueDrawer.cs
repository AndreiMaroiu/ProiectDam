using Core;
using Core.Events.Binding;
using Core.Values;
using UnityEditor;
using UnityEngine;

namespace EditorScripts
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

    [CustomPropertyDrawer(typeof(BindableValue<int>))]
    public sealed class IntBindableValueDrawer : BindableValueDrawer<BindableValue<int>>
    {

    }

    [CustomPropertyDrawer(typeof(BindableValue<bool>))]
    public sealed class BoolBindableValueDrawer : BindableValueDrawer<BindableValue<bool>>
    {

    }

    [CustomPropertyDrawer(typeof(BindableValue<Room>))]
    public sealed class RoomBindableValueDrawer : BindableValueDrawer<BindableValue<Room>>
    {

    }

    [CustomPropertyDrawer(typeof(BindableValue<BiomeType>))]
    public sealed class BiomeBindableValueDrawer : BindableValueDrawer<BindableValue<BiomeType>>
    {

    }
}
