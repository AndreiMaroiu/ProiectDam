using Gameplay.Events;
using Gameplay.Generation;
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
        [SerializeField] private BiomeEvent _currentBiome;

        private SpriteRenderer _renderer;

        private void Awake()
        {
            _currentBiome.OnValueChanged += OnBiomeChanged;

            _renderer = GetComponent<SpriteRenderer>();
        }

        private void OnDestroy()
        {
            _currentBiome.OnValueChanged -= OnBiomeChanged;
        }

        private void OnBiomeChanged()
        {
            Sprite sprite = _currentBiome.Value switch
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
