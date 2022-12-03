using UnityEngine;

namespace Core.Events
{
    [CreateAssetMenu(fileName = "New Room Event", menuName = "Scriptables/Events/Room Event")]
    public sealed class RoomEvent : BaseValueEvent<Room>
    {
        public Room StartRoom { get; set; }
    }
}
