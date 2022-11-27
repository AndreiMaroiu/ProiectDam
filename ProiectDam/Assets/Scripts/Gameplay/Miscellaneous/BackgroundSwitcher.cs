using Core.Events;
using Core.Values;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BackgroundSwitcher : MonoBehaviour
    {
        [SerializeField] private Sprite _grass;
        [SerializeField] private Sprite _fire;
        [SerializeField] private Sprite _dungeon;
        [Header("Events")]
        [SerializeField] private LayerEvent _layerEvent;

        private SpriteRenderer _renderer;

        private void Awake()
        {
            _layerEvent.CurrentBiome.OnValueChanged += OnBiomeChanged;

            _renderer = GetComponent<SpriteRenderer>();
        }

        private void OnDestroy()
        {
            _layerEvent.CurrentBiome.OnValueChanged -= OnBiomeChanged;
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
