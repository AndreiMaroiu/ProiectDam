using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameScreenManager : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] private Slider _layerSlider;
        [Header("Events")]
        [SerializeField] private IntEvent _layersCountEvent;
        [SerializeField] private IntEvent _currentLayerEvent;

        private void Start()
        {
            _layersCountEvent.OnValueChanged += OnLayersCountChanged;
            _layerSlider.onValueChanged.AddListener(OnSliderChanged);

            OnLayersCountChanged();
        }

        private void OnLayersCountChanged()
        {
            _layerSlider.gameObject.SetActive(_layersCountEvent.Value > 1);
            _layerSlider.maxValue = _layersCountEvent.Value - 1;
            _layerSlider.value = _currentLayerEvent.Value;
        }

        private void OnSliderChanged(float value)
        {
            _currentLayerEvent.Value = Mathf.RoundToInt(value);
            _layerSlider.value = Mathf.RoundToInt(value);
        }

        private void OnDestroy()
        {
            _layersCountEvent.OnValueChanged -= OnLayersCountChanged;
            _layerSlider.onValueChanged.RemoveListener(OnSliderChanged);
        }
    }
}
