using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Core.Events;
using Core.Events.Binding;
using Core.Values;

namespace Gameplay
{
    public sealed class HubPlayerController : BaseBindableBehaviour
    {
        [SerializeField] private HubPointEvent _hubPointEvent;

        private Coroutine _coroutine;

        private void Start()
        {
            Bind(_hubPointEvent, OnHubPointChanged);
        }

        private void OnHubPointChanged(HubPoint point)
        {
            if (_coroutine is not null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(MoveToPoint());   
        }

        private IEnumerator MoveToPoint()
        {
            yield break;
        }
    }
}
