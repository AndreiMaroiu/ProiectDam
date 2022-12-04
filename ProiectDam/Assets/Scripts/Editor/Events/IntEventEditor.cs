#if UNITY_EDITOR

using Core.Events;
using UnityEditor;

namespace EditorScripts.Events
{
    [CustomEditor(typeof(IntEvent))]
    public class IntEventEditor : BaseEventEditor<IntEvent, int>
    {

    }
}

#endif