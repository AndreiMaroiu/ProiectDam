using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class LoadingScreenManager : MonoBehaviour
    {
        private Animator _animator;
        private const string WALK_ANIMATION = "Walk";

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.SetBool(WALK_ANIMATION, true);
        }
    }
}
