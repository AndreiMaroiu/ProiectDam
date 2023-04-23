using Core.Events.Binding;

namespace Core.Quests
{
    public abstract class Quest
    {
        public Quest(int target, int progress = 0)
        {
            _target = target;
            Progress = new(progress);
        }

        protected int _target;

        public bool Completed => _target <= Progress;

        public BindableValue<int> Progress { get; set; }


        /// <summary>
        /// Init quest data, called on start
        /// </summary>
        public abstract void InitQuest(QuestEvents events);

        public abstract void Clean(QuestEvents events);
    }
}
