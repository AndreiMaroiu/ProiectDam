using Gameplay.Generation;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace EditorScripts
{
    public class TileSettingsSerializationInfo : SerializationInfoHelper<TileSettings>
    {
        public static TileSettingsSerializationInfo Instance { get; } = new();

        protected TileSettingsSerializationInfo() { }

        protected override bool TypeCheck(FieldInfo field)
        {
            return field.FieldType == typeof(TileData[]);
        }
    }

    public class TileDataSerializationInfo : SerializationInfoHelper<TileData>
    {
        public static TileDataSerializationInfo Instance { get; } = new();

        protected TileDataSerializationInfo() { }

        protected override bool TypeCheck(FieldInfo field)
        {
            return true;
        }
    }

    public abstract class SerializationInfoHelper<T>
    {
        protected SerializationInfoHelper()
        {
            Type type = typeof(T);

            FieldInfo[] serializablesFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.GetCustomAttribute<SerializeField>() is not null && TypeCheck(f))
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

        public string[] DisplayNames { get; }

        public string[] InternalNames { get; }

        protected abstract bool TypeCheck(FieldInfo field);

        private static string GenerateDisplayName(string name)
        {
            string temp = name.Replace("_", "");
            return char.ToUpper(temp[0]).ToString() + temp[1..];
        }
    }
}
