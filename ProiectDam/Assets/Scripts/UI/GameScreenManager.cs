using Core.Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameScreenManager : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] private Slider _layerSlider;
        [SerializeField] private Button _previewButton;
        [SerializeField] private Button _weaponButton;
        [SerializeField] private Button _meleeButton;
        [Header("Events")]
        [SerializeField] private IntEvent _layersCountEvent;
        [SerializeField] private IntEvent _currentLayerEvent;
        [SerializeField] private BoolEvent _previewActive;
        [SerializeField] private BoolEvent _playerTurn;
        [SerializeField] private GameEvent _meleeEvent;
        [SerializeField] private GameEvent _shootEvent;
        [Header("Texts")]
        [SerializeField] private Text _previewText;
        [SerializeField] private Text _previewLabel;

        public float LastTimeScale { get; set; }

        public const float StoppedScale = 0.0f;

        private void Start()
        {
            _layersCountEvent.OnValueChanged += OnLayersCountChanged;
            _currentLayerEvent.OnValueChanged += OnCurrentLayerChanged;
            _layerSlider.onValueChanged.AddListener(OnSliderChanged);

            _previewActive.OnValueChanged += OnPreviewChanged;

            _layerSlider.gameObject.SetActive(false);
            _previewButton.gameObject.SetActive(false);
            _previewLabel.gameObject.SetActive(false);

            LastTimeScale = Time.timeScale;

            OnLayersCountChanged();
        }

        private void OnLayersCountChanged()
        {
            _layerSlider.maxValue = _layersCountEvent.Value - 1;
            _layerSlider.value = _currentLayerEvent.Value;
            if (_layersCountEvent.Value > 1)
            {
                _previewButton.gameObject.SetActive(true);
                _previewLabel.gameObject.SetActive(true);
            }
            else
            {
                _previewButton.gameObject.SetActive(false);
                _previewLabel.gameObject.SetActive(false);
            }
        }

        private void OnCurrentLayerChanged()
        {
            _layerSlider.value = _currentLayerEvent.Value;
        }

        private void OnSliderChanged(float value)
        {
            _currentLayerEvent.Value = Mathf.RoundToInt(value);
            _layerSlider.value = Mathf.RoundToInt(value);
        }

        private void OnPreviewChanged()
        {
            if (_previewActive.Value && _playerTurn.Value)
            {
                LastTimeScale = Time.timeScale;
                Time.timeScale = StoppedScale;
                _layerSlider.gameObject.SetActive(true);
                _weaponButton.gameObject.SetActive(false);
                _meleeButton.gameObject.SetActive(false);
                _previewText.text = "ON";
            }
            else
            { 
                _layerSlider.gameObject.SetActive(false);
                _weaponButton.gameObject.SetActive(true);
                _meleeButton.gameObject.SetActive(true);
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
            _currentLayerEvent.OnValueChanged -= OnCurrentLayerChanged;
            _layerSlider.onValueChanged.RemoveListener(OnSliderChanged);
            _previewActive.OnValueChanged -= OnPreviewChanged;
        }

        public void OnWeaponClick()
        {
            _shootEvent.Invoke();
        }

        public void OnMeleeClick()
        {
            _meleeEvent.Invoke();
        }
    }
}
