using Gameplay.Generation;
using UnityEditor;
using UnityEngine;

namespace EditorScripts
{
    public class ShowPopupExample : EditorWindow
    {
        public static void Show(Object target)
        {
            ShowPopupExample window = ScriptableObject.CreateInstance<ShowPopupExample>();
            window.Init(target);

            window.Show();
        }

        private SerializedObject target;

        private int _selectedIndex;

        private void Init(Object target)
        {
            this.target = new(target);
            _selectedIndex = 0;
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Drag and drop window", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (_selectedIndex > TileSettingsSerializationInfo.Instance.DisplayNames.Length)
            {
                _selectedIndex = 0;
            }

            _selectedIndex = EditorGUILayout.Popup("Tile Type", _selectedIndex, TileSettingsSerializationInfo.Instance.DisplayNames);

            if (target is null)
            {
                return;
            }

            SerializedProperty prop = target.FindProperty(TileSettingsSerializationInfo.Instance.InternalNames[_selectedIndex]);
            EditorGUILayout.PropertyField(prop);

            switch (Event.current.type)
            {
                case EventType.DragUpdated:
                    HandleDragUpdated();
                    break;
                case EventType.DragPerform:
                    HandleDragPerformed(target, prop);
                    break;
            }

            GUILayout.Label("Drag here");

            target.ApplyModifiedProperties();
            
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
    }
}
