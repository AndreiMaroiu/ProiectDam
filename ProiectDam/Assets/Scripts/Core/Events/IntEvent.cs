using System;
using UnityEngine;

namespace Core.Events
{
    [CreateAssetMenu(fileName = "New Int Event", menuName = "Scriptables/Events/Int Event")]
    public sealed class IntEvent : BaseValueEvent<int>
    {

    }
}
