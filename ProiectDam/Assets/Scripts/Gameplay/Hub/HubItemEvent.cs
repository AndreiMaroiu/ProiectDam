using Core.Events;
using Gameplay.PickUps;
using UnityEngine;

namespace Gameplay.Hub
{
    public enum HubItemType
    {
        None,
        Temporary,
        Permanent
    }

    public class HubEventInfo
    {
        public Item Item { get; set; }
        public HubItemType Type { get; set; }

    }

    [CreateAssetMenu(fileName = "Hub Item Event", menuName = "Scriptables/Hub/Item Event")]
    public class HubItemEvent : BaseEvent<HubEventInfo>
    {

    }
}
