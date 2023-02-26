using Core.Events;
using UnityEngine;

namespace UI.Hub
{
    [CreateAssetMenu(menuName = "Scriptables/Hub/Description Event", fileName = "Hub Point Description Event")]
    public sealed class HubPointDescriptionEvent : BaseValueEvent<string>
    {

    }
}
