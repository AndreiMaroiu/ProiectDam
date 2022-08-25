using UnityEngine;

namespace Core.Items
{
    public abstract class ItemDescription : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private int _cost;
        [SerializeField] private Sprite _image;
        [SerializeField] private string _description;
        [SerializeField] private AudioClip _sound;

        public string Name => _name;
        public int Cost => _cost;
        public Sprite Image => _image;
        public string Description => _description;
        public AudioClip Sound => _sound;
    }
}
