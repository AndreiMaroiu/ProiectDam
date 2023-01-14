#if UNITY_EDITOR

using Gameplay.Generation;
using UnityEditor;
using UnityEngine;

namespace EditorScripts
{
    [CustomPropertyDrawer(typeof(TileData))]
    public class TileDataDrawer : PropertyDrawer
    {
        private const int _propCount = 4;

        private const float _heightDistance = 10;
        private const float _oneThirdHeigh = _heightDistance / _propCount;

        private static readonly GUIContent _flagsGUIContent = new("Flags", "Select in which room types can spawn the prefab");
        private static readonly GUIContent _chanceGUIContent = new("Spawn Chance");
        private static readonly GUIContent _onlyOnceGUIContent = new("Only once", "If you, the tile will spawn only once per room");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float halfHeight = position.height / 4 - _oneThirdHeigh;

            Rect prefabRect = new(position.x, position.y, position.width, halfHeight);
            Rect flagsRect = new(position.x, GetY(position, 1), position.width, halfHeight);
            Rect chanceRect = new(position.x, GetY(position, 2), position.width, halfHeight);
            Rect onlyOnceRect = new(position.x, GetY(position, 3), position.width, halfHeight);

            SerializedProperty prefab = property.FindPropertyRelative("_prefab");
            SerializedProperty flags = property.FindPropertyRelative("_flags");
            SerializedProperty chance = property.FindPropertyRelative("_spawnChance");
            SerializedProperty onlyOnce = property.FindPropertyRelative("_onlyOnce");

            EditorGUI.PropertyField(prefabRect, prefab, GUIContent.none);
            EditorGUI.PropertyField(flagsRect, flags, _flagsGUIContent);
            EditorGUI.PropertyField(chanceRect, chance, _chanceGUIContent);
            EditorGUI.PropertyField(onlyOnceRect, onlyOnce, _onlyOnceGUIContent);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * _propCount + _heightDistance;
        }

        private static float GetY(Rect position, int i) // the number of the field
        {
            float halfHeight = position.height / _propCount;
            return position.y + halfHeight * i;
        }
    }
}

#endif
