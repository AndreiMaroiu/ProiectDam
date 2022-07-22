using UnityEngine;

namespace Core.Items
{
    public abstract class ItemDescription : ScriptableObject
    {
        [SerializeField] private int _cost;
        [SerializeField] private Sprite _image;
        [SerializeField] private string _description;
        
        public int Cost => _cost;
        public Sprite Image => _image;
        public string Description => _description;
    }
}
