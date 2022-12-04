#if UNITY_EDITOR

using Core.Events.Binding;
using UnityEditor;

namespace EditorScripts.Events
{
    [CustomPropertyDrawer(typeof(BindableValue<int>))]
    public sealed class IntBindableValueDrawer : BindableValueDrawer<BindableValue<int>>
    {

    }
}

#endif
