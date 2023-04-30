using Core.Services;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Core
{
    public sealed class StaticServices
    {
        private static StaticServices Instance { get; } = new();

        private readonly Dictionary<Type, object> _services = new();

        private StaticServices()
        {
            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.sceneUnloaded += SceneUnloaded;
        }

        private void SceneUnloaded(Scene scene)
        {
            foreach (var service in _services.Values)
            {
                if (service is ISceneAwareService sceneAware)
                {
                    sceneAware.OnSceneUnloaded(scene);
                }
            }
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            foreach (var service in _services.Values)
            {
                if (service is ISceneAwareService sceneAware)
                {
                    sceneAware.OnSceneLoaded(scene);
                }
            }
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

        public static void Set<T>(T service) where T : class => Instance._services[typeof(T)] = service;

        public static bool IsPresent<T>() => Instance._services.ContainsKey(typeof(T));

        public static bool Delete<T>() => Instance._services.Remove(typeof(T));

        public static void Clear()
        {
            Instance._services.Clear();
        }
    }
}
