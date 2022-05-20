using Gameplay.PickUps;
using System;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace EditorScripts
{
    [CustomEditor(typeof(PickUp))]
    public class PickUpEditor : Editor
    {
        private SerializedProperty type;
        private string[] _options = { };
        private int _index;

        private void OnEnable()
        {
            type = serializedObject.FindProperty("_type");

            var assembly = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => a.GetName().Name == "Gameplay")
                .FirstOrDefault();

            Debug.Log("Is assembly null: " + (assembly is null));

            if (assembly is null)
            {
                return;
            }

            var types = assembly
                .GetTypes()
                .Where(type => {
                    return type.IsSubclassOf(typeof(AbstractPickUp))
                        && type.IsAbstract is false;
                })
                .ToArray();

            _options = new string[types.Length];

            for(int i = 0; i < types.Length; i++)
            {
                PickUpDataAttribute attribute = Attribute.GetCustomAttribute(types[i], typeof(PickUpDataAttribute)) as PickUpDataAttribute;
                _options[i] = (attribute is null) ? types[i].Name : attribute.Name;
            }

            if (type is null)
            {
                Debug.LogError("No type found!");
            }
        }

        public override void OnInspectorGUI()
        {
            _index = EditorGUILayout.Popup("Type", _index, _options);

            if (type != null)
            {
                type.stringValue = _options[_index];
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
