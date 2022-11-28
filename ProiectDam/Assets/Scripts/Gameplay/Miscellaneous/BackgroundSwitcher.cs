using Core.Events;
using Core.Values;
using UnityEngine;
using Core.Events.Binding;

namespace Gameplay
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BackgroundSwitcher : BaseBindableBehaviour
    {
        [SerializeField] private Sprite _grass;
        [SerializeField] private Sprite _fire;
        [SerializeField] private Sprite _dungeon;
        [Header("Events")]
        [SerializeField] private LayerEvent _layerEvent;

        private SpriteRenderer _renderer;

        private void Awake()
        {
            Bind(_layerEvent.CurrentBiome, OnBiomeChanged);

            _renderer = GetComponent<SpriteRenderer>();
        }

        private void OnBiomeChanged(BiomeType biomeType)
        {
            Sprite sprite = biomeType switch
            {
                BiomeType.Fire => _fire,
                BiomeType.Grassland => _grass,
                BiomeType.Dungeon => _dungeon,
                _ => null
            };

            _renderer.sprite = sprite;
        }
    }
}
