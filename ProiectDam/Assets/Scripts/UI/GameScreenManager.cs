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

        [SerializeField] private BoolEvent _previewActive;
        [SerializeField] private Text _previewText;

        public float LastTimeScale { get; set; }

        public const float StoppedScale = 0.0f;

        private void Start()
        {
            _layersCountEvent.OnValueChanged += OnLayersCountChanged;
            _layerSlider.onValueChanged.AddListener(OnSliderChanged);

            _previewActive.OnValueChanged += OnPreviewChanged;

            _layerSlider.gameObject.SetActive(false);
            LastTimeScale = Time.timeScale;

            OnLayersCountChanged();
        }

        private void OnLayersCountChanged()
        {
            _layerSlider.maxValue = _layersCountEvent.Value - 1;
            _layerSlider.value = _currentLayerEvent.Value;
        }

        private void OnSliderChanged(float value)
        {
            _currentLayerEvent.Value = Mathf.RoundToInt(value);
            _layerSlider.value = Mathf.RoundToInt(value);
        }

        private void OnPreviewChanged()
        {
            if (!_previewActive.Value)
            {
                Time.timeScale = StoppedScale;
                _layerSlider.gameObject.SetActive(_layersCountEvent.Value > 1);
                _previewText.text = "ON";
            }
            else
            { 
                _layerSlider.gameObject.SetActive(false);
                _previewText.text = "OFF";
                Time.timeScale = LastTimeScale;
            }
        }

        public void OnPreviewClick()
        {
            _previewActive.Value = !_previewActive.Value;
        }

        private void OnDestroy()
        {
            _layersCountEvent.OnValueChanged -= OnLayersCountChanged;
            _layerSlider.onValueChanged.RemoveListener(OnSliderChanged);
            _previewActive.OnValueChanged -= OnPreviewChanged;
        }
    }
}
