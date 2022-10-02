﻿#if UNITY_EDITOR

using Gameplay.Generation;
using UnityEditor;
using UnityEngine;

namespace EditorScripts
{
    public class TileSettingsHelpingWindow : EditorWindow
    {
        public static void Show(Object target)
        {
            TileSettingsHelpingWindow window = ScriptableObject.CreateInstance<TileSettingsHelpingWindow>();
            window.Init(target);

            window.Show();
        }

        private static readonly string[] _options = new string[] { "Drag and Drop Tiles", "Set All Fields" };

        private SerializedObject target;
        private int _selectedIndex;
        private int _optionsIndex;
        private int _tileDataOption;
        private Vector2 _scrollPos;
        private bool _forceExpand;

        private void Init(Object target)
        {
            this.target = new(target);
            _selectedIndex = 0;
        }

        private void OnGUI()
        {
            CenterHorizontal(() => GUILayout.Label("TileSettings helper", EditorStyles.boldLabel));

            _optionsIndex = EditorGUILayout.Popup("Action", _optionsIndex, _options);
            _selectedIndex = EditorGUILayout.Popup("Tile Type", _selectedIndex, TileSettingsSerializationInfo.Instance.DisplayNames);
            _forceExpand = EditorGUILayout.Toggle("Force expand", _forceExpand);
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            if (target is null)
            {
                return;
            }

            SerializedProperty prop = target.FindProperty(TileSettingsSerializationInfo.Instance.InternalNames[_selectedIndex]);
            prop.isExpanded = _forceExpand || prop.isExpanded;

            EditorGUILayout.PropertyField(prop);

            switch (_optionsIndex)
            {
                case 0: // drag and drop
                    EditorGUILayout.EndScrollView();
                    CenterHorizontal(() => GUILayout.Label("Drag Anywhere"));
                    HandleDragAndDrop(prop);
                    break;
                case 1: // set all fields
                    HandleSet(prop);
                    EditorGUILayout.EndScrollView();
                    break;
            }

            target.ApplyModifiedProperties();
        }

        private void HandleSet(SerializedProperty prop)
        {
            if (prop.arraySize < 1)
            {
                CenterHorizontal(() => GUILayout.Label("No data to set!"));
                return;
            }

            CenterHorizontal(() => GUILayout.Label("Edit all elements in array"));
            _tileDataOption = EditorGUILayout.Popup("Tile Data field", _tileDataOption, TileDataSerializationInfo.Instance.DisplayNames);

            string internalFieldName = TileDataSerializationInfo.Instance.InternalNames[_tileDataOption];

            var firstProp = prop.GetArrayElementAtIndex(0).FindPropertyRelative(internalFieldName);

            EditorGUILayout.PropertyField(firstProp);

            if (GUILayout.Button("Apply changes!"))
            {
                for (int i = 1; i < prop.arraySize; i++)
                {
                    SerializedProperty current = prop.GetArrayElementAtIndex(i);
                    SerializedProperty field = current.FindPropertyRelative(internalFieldName);

                    switch (field.type)
                    {
                        case "int":
                            field.intValue = firstProp.intValue;
                            break;
                        case "Enum":
                            field.enumValueFlag = firstProp.enumValueFlag;
                            break;
                        case "PPtr<$GameObject>":
                            Debug.LogWarning("Should not set value for this type!");
                            break;
                        default:
                            break;
                    }

                    Debug.Log(field.type);
                }
            }
        }

        private void HandleDragAndDrop(SerializedProperty prop)
        {
            switch (Event.current.type)
            {
                case EventType.DragUpdated:
                    HandleDragUpdated();
                    break;
                case EventType.DragPerform:
                    HandleDragPerformed(target, prop);
                    break;
            }
        }

        private void HandleDragUpdated()
        {
            if (!IsDragDataPrefabs(DragAndDrop.objectReferences))
            {
                return;
            }

            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            Event.current.Use();
        }

        private void HandleDragPerformed(SerializedObject target, SerializedProperty prop)
        {
            if (!IsDragDataPrefabs(DragAndDrop.objectReferences))
            {
                return;
            }

            DragAndDrop.AcceptDrag();

            int startIndex = prop.arraySize; // start from the last element
            int dragIndex = 0;
            prop.arraySize += DragAndDrop.objectReferences.Length;

            for (int i = startIndex; i < prop.arraySize; i++)
            {
                SerializedProperty current = prop.GetArrayElementAtIndex(i);
                SerializedProperty prefab = current.FindPropertyRelative("_prefab");
                SerializedProperty flags = current.FindPropertyRelative("_flags");
                SerializedProperty chance = current.FindPropertyRelative("_spawnChance");

                prefab.objectReferenceValue = DragAndDrop.objectReferences[dragIndex++];
                flags.enumValueFlag = TileData.DefaultFlags;
                chance.intValue = TileData.DefaultSpawnChance;
            }

            target.ApplyModifiedProperties();
        }

        private bool IsDragDataPrefabs(Object[] objects)
        {
            foreach (var item in objects)
            {
                if (item is not GameObject)
                {
                    return false;
                }
            }

            return true;
        }

        private static void CenterHorizontal(System.Action action)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            action();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}


#endif