using Core.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Quests
{
    [CreateAssetMenu(fileName = "NewKillEnemiesQuest", menuName = "Scriptables/KillEnemiesQuest")]
    public class KillEnemiesQuest : Quest
    {
        [SerializeField] private GameEvent _enemyKilledEvent;

        public override event Action<int> OnProgressChanged;

        public override void InitQuest()
        {
            
        }

        public void OnEnable()
        {
            Debug.Log("quest enable");
        }

        public void Awake()
        {
            Debug.Log("quest awake");
        }

        public void OnDestroy()
        {
            Debug.Log("quest destroy");
        }
    }
}
