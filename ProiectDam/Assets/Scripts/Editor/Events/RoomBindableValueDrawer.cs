using Core;
using Core.Events.Binding;
using UnityEditor;

namespace EditorScripts.Events
{
    [CustomPropertyDrawer(typeof(BindableValue<Room>))]
    public sealed class RoomBindableValueDrawer : BindableValueDrawer<BindableValue<Room>>
    {

    }
}
