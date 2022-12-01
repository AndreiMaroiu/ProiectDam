using Core.Events.Binding;
using Core.Values;
using UnityEngine;

namespace Core.Events
{
    [CreateAssetMenu(fileName = "New Layer Event", menuName = "Scriptables/Events/Layer Event")]
    public sealed class LayerEvent : ScriptableObject
    {
        public BindableValue<int> LayerCount { get; } = new();
        public BindableValue<int> CurrentLayer { get; } = new();
        public BindableValue<BiomeType> CurrentBiome { get; } = new();
    }
}
