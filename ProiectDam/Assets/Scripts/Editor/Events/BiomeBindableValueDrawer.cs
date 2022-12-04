#if UNITY_EDITOR

using Core.Events.Binding;
using Core.Values;
using UnityEditor;

namespace EditorScripts.Events
{
    [CustomPropertyDrawer(typeof(BindableValue<BiomeType>))]
    public sealed class BiomeBindableValueDrawer : BindableValueDrawer<BindableValue<BiomeType>>
    {

    }
}

#endif
