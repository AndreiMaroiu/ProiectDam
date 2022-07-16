using Core;
using UnityEngine;

namespace Gameplay.PickUps
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Scriptables/Items/Item")]
    public class Item : ItemDescription
    {
        [SerializeField] private PickUpHandler _type;
        [SerializeField] private int _points = 1;

        public int Points => _points;
        public string Type => _type.Type;

#if UNITY_EDITOR
        public AbstractPickUp GetPickUp() => new PickUpFactory().GetPickUp(this);
#else
        public AbstractPickUp GetPickUp() => PickUpFactory.Instance.GetPickUp(this);
#endif
    }
}
