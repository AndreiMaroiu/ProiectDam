using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Scriptables/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private int _cost;
        [SerializeField] private Sprite _image;
        [SerializeField] private string _description;
        
        public int Cost => _cost;
        public Sprite Image => _image;
        public string Description => _description;
    }
}
