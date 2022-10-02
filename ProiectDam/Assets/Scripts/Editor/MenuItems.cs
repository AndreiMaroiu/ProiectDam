#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.IO;

namespace EditorScripts
{
    public static class MenuItems
    {
        [MenuItem("Tools/Saved Data/Clear Player Prefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Tools/Saved Data/Clear all saves and stats")]
        public static void ClearAllPersistentFiles()
        {
            string[] files = Directory.GetFiles(Application.persistentDataPath + "/");
            foreach (var file in files)
            {
                File.Delete(file);
            }

            Debug.LogWarning($"Deleted {files.Length.ToString()} file(s)");
        }
    }
}

#endif