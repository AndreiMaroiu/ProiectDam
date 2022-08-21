using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Quests
{
    public abstract class Quest : ScriptableObject
    {
        [SerializeField] protected int _target;

        public abstract event Action<int> OnProgressChanged;

        public bool Completed => _target == Progress;

        public virtual int Progress { get; protected set; }

        /// <summary>
        /// Init quest data, called on start
        /// </summary>
        public virtual void InitQuest()
        {

        }

        public virtual void Clean()
        {

        }
    }
}
