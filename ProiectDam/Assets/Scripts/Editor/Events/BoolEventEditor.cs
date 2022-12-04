using Core.Events;
using UnityEditor;

namespace EditorScripts.Events
{
    [CustomEditor(typeof(BoolEvent))]
    public class BoolEventEditor : BaseEventEditor<BoolEvent, bool>
    {

    }
}
