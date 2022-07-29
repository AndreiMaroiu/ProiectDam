using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "New Map Icons", menuName = "Scriptables/MapIcons")]
    public class MapIcons : ScriptableObject
    {
        [SerializeField] private Sprite _start;
        [SerializeField] private Sprite _end;
        [SerializeField] private Sprite _healing;
        [SerializeField] private Sprite _chest;
        [SerializeField] private Sprite _merchant;


        public Sprite GetIcon(RoomType type) => type switch
        {
            RoomType.Empty => null,
            RoomType.Normal => null,
            RoomType.Start => _start,
            RoomType.End => _end,
            RoomType.Healing => _healing,
            RoomType.Chest => _chest,
            RoomType.Wall => null,
            RoomType.Merchant => _merchant,
            _ => null,
        };
    }
}
