using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    private AudioSource[] mySounds;
    private AudioSource move;

    void Start()
    {
        mySounds = GetComponents<AudioSource>();
        move = mySounds[0];
    }
    public void PlayMove()
    { move.Play(); }
}
