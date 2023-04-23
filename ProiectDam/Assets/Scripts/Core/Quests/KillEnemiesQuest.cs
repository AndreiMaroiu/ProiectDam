using Core.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Quests
{
    public class KillEnemiesQuest : Quest
    {
        public KillEnemiesQuest(int target, int value) : base(target, value)
        {
                
        }

        public override void Clean(QuestEvents events)
        {
            events.GlobalDeathEvent.OnEvent += GlobalDeathEvent_OnEvent;
        }

        private void GlobalDeathEvent_OnEvent(object sender)
        {
            Progress.Value += 1;
        }

        public override void InitQuest(QuestEvents events)
        {
            events.GlobalDeathEvent.OnEvent += GlobalDeathEvent_OnEvent;
        }
    }
}
