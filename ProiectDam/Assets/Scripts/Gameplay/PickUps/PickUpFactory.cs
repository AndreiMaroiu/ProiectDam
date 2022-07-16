using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

namespace Gameplay.PickUps
{
    public class PickUpFactory
    {
        public static PickUpFactory Instance { get; } = new PickUpFactory();

        private readonly Dictionary<string, Type> _pickUps;

        public PickUpFactory() 
        {
            Assembly assembly = typeof(PickUpFactory).Assembly;

            IEnumerable<Type> types = assembly.GetTypes()
                .Where(type => typeof(AbstractPickUp).IsAssignableFrom(type) && !type.IsAbstract);

            _pickUps = types.ToDictionary(type => type.Name);
            Names = _pickUps.Keys.ToArray();
        }

        public string[] Names { get; }

        public AbstractPickUp GetPickUp(Item item) 
            => GetPickUp(item.Type, item.Points);

        public AbstractPickUp GetPickUp(string pickUpType, int value) 
            => Activator.CreateInstance(_pickUps[pickUpType], value) as AbstractPickUp;
    }
}
