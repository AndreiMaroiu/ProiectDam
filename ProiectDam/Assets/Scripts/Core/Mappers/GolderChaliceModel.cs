using Core.Items;
using System;
using UnityEngine;

namespace Core.Mappers
{
    public class GolderChaliceModel : IButtonModel
    {
        public GameObject Owner { get; set; }

        public Func<ItemDescription> Action { get; set; }

        public int CurrentScore { get; set; }
        public int MinScore { get; set; }
    }
}
