using Core.DataSaving;
using UnityEngine;

namespace Core.Events
{
    [CreateAssetMenu(fileName = "New Save Event", menuName = "Scriptables/Events/SaveEvent")]
    public class SaveEvent : BaseEvent<SaveType>
    {

    }
}
