using Gameplay.Generation;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace EditorScripts
{
    public class TileSettingsSerializationInfo
    {
        public static TileSettingsSerializationInfo Instance { get; } = new();

        public string[] DisplayNames { get; }

        public string[] InternalNames { get; }

        public TileSettingsSerializationInfo()
        {
            Type type = typeof(TileSettings);

            FieldInfo[] serializablesFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.GetCustomAttribute<SerializeField>() is not null && f.FieldType == typeof(TileData[]))
                .ToArray();

            int count = serializablesFields.Length;

            DisplayNames = new string[count];
            InternalNames = new string[count];

            for (int i = 0; i < count; i++)
            {
                string internalName = serializablesFields[i].Name;
                InternalNames[i] = internalName;
                DisplayNames[i] = GenerateDisplayName(internalName);
            }
        }

        private static string GenerateDisplayName(string name)
        {
            string temp = name.Replace("_", "");
            return char.ToUpper(temp[0]).ToString() + temp[1..];
        }
    }
}
