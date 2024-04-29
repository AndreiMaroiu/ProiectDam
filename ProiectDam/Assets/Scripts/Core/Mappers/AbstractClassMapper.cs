using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Base class to map a class instance to an scriptable object
    /// </summary>

    public abstract class AbstractClassMapper<ClassType, ScriptableType> : ScriptableObject where ScriptableType : ScriptableObject where ClassType : class
    {
        [System.Serializable]
        public sealed class MapData
        {
            [SerializeField] private string _className;
            [SerializeField] private ScriptableType _name;

            private readonly Lazy<Guid> _guid = new(() => TypeOfClass.GetCustomAttribute<ClassIDAttribute>().Guid);

            public Guid Guid => _guid.Value;
        }

        public static Type TypeOfClass => typeof(ClassType);
        public static Type TypeOfScriptable => typeof(ScriptableType);

        [SerializeField] private MapData[] _map;

        private Dictionary<Guid, MapData> _mapData;

        private void OnEnable()
        {
            
        }

        // how this hsould behave: you should implement from this class to have a mapper, also you should implement some abstract classes to retrieve the data
    }

    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class ClassIDAttribute : Attribute
    {
        private readonly string _guid;

        public ClassIDAttribute(string guid)
        {
            _guid = guid;
        }

        public Guid Guid => Guid.Parse(_guid);
    }
}
