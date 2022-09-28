using Gameplay;
using Gameplay.Generation;
using UnityEditor;
using UnityEngine;

namespace EditorScripts
{
    [CustomEditor(typeof(TileSettings)), CanEditMultipleObjects]
    public class TileSettingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Show drag and drop options"))
            {
                TileSettingsHelpingWindow.Show(target);
            }
        }
    }
}
