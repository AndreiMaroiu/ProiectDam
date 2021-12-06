using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class LoadingScreenManager : MonoBehaviour
    {
        private Animator _animator;
        private const string WALK_ANIMATION = "Walk";
        void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.SetBool(WALK_ANIMATION, true);
        }
    }
}
