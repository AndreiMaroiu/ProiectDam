using System;

namespace Gameplay.PickUps
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PickUpDataAttribute : Attribute
    {
        public PickUpDataAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
