using System;
using System.Collections.Generic;

namespace Core
{
    public sealed class StaticServices
    {
        private static StaticServices Instance { get; } = new();

        private readonly Dictionary<Type, object> _services = new();

        private StaticServices()
        {

        }

        public static T Get<T>() where T : class, new()
        {
            if (Instance._services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }

            service = Activator.CreateInstance(typeof(T));
            Instance._services[typeof(T)] = service;

            return (T)service;
        }
        public static void Clear()
        {
            Instance._services.Clear();
        }
    }
}
