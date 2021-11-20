using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Sound
{
    public class SoundHandler : MonoBehaviour
    {
        private AudioSource[] mySounds;
        private AudioSource move;

        private void Start()
        {
            mySounds = GetComponents<AudioSource>();
            move = mySounds[0];
        }

        public void PlayMove() => move.Play();

        public void StopMove() => move.Stop();
    }
}
