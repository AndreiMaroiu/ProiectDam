using Core.Events.Binding;
using UnityEditor;

namespace EditorScripts.Events
{
    [CustomPropertyDrawer(typeof(BindableValue<bool>))]
    public sealed class BoolBindableValueDrawer : BindableValueDrawer<BindableValue<bool>>
    {

    }
}
