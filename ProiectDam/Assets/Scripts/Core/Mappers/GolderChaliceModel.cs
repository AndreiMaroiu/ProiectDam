using Core.Items;
using System;
using UnityEngine.Events;

namespace Core.Mappers
{
    public class GolderChaliceModel : IButtonModel
    {
        public Func<ItemDescription> Action { get; set; }

        public int CurrentScore { get; set; }
        public int MinScore { get; set; }
    }
}
