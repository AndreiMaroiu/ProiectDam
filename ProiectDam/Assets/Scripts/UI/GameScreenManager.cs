using Core.Events;
using Core.Events.Binding;
using Core.Mappers;
using System.Collections.Generic;
using UI.ButtonViews;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameScreenManager : BaseBindableBehaviour
    {
        [Header("Objects")]
        [SerializeField] private Slider _layerSlider;
        [SerializeField] private Button _previewButton;
        [SerializeField] private Button _weaponButton;
        [SerializeField] private Button _meleeButton;
        [SerializeField] private Button _middleButton;
        [SerializeField] private Text _middleButtonText;
        [Header("Events")]
        [SerializeField] private LayerEvent _layersEvent;
        [SerializeField] private BoolEvent _previewActive;
        [SerializeField] private BoolEvent _playerTurn;
        [SerializeField] private GameEvent _meleeEvent;
        [SerializeField] private GameEvent _shootEvent;
        [SerializeField] private ButtonEvent _buttonEvent;
        [Header("Texts")]
        [SerializeField] private Text _previewText;
        [SerializeField] private Text _previewLabel;

        private List<IButtonModel> _buttonInfos;
        private ButtonMapper _buttonMapper;

        public float LastTimeScale { get; set; }

        public const float StoppedScale = 0.0f;

        private void Start()
        {
            Bind(_layersEvent.LayerCount, OnLayersCountChanged);
            Bind(_layersEvent.CurrentLayer, OnCurrentLayerChanged);
            Bind(_previewActive, OnPreviewChanged);
            Bind(_layerSlider.onValueChanged, OnSliderChanged);
            Bind(_buttonEvent.OnShowBindable, OnShowButton);
            Bind(_buttonEvent.OnCloseBindable, OnCloseButton);
            Bind(_buttonEvent.OnPressBindable, OnObjectPressed);

            _layerSlider.gameObject.SetActive(false);
            _previewButton.gameObject.SetActive(false);
            _previewLabel.gameObject.SetActive(false);
            _middleButton.gameObject.SetActive(false);

            LastTimeScale = Time.timeScale;

            OnLayersCountChanged();

            _buttonInfos = new();
            _buttonMapper = new();
            _buttonMapper.Map(typeof(SimpleButtonModel), new SimpleView());
            _buttonMapper.Map(typeof(GolderChaliceModel), new GoldenChaliceView());
            _buttonMapper.TargetButton = new()
            {
                Button = _middleButton,
                Text = _middleButtonText,
                Icon = null, // TODO:
            };
        }

        private void OnObjectPressed(GameObject sender)
        {
            foreach (var model in _buttonInfos)
            {
                if (model.Owner == sender)
                {
                    OnShowButton(model);
                    _middleButton.onClick.Invoke();
                    break;
                }
            }
        }

        private void OnShowButton(IButtonModel info)
        {
            _buttonMapper.Setup(info);
            _buttonInfos.Add(info);

            _middleButton.gameObject.SetActive(true);
        }

        private void OnCloseButton(IButtonModel info)
        {
            _middleButton.onClick.RemoveAllListeners();
            _buttonInfos.Remove(info);

            if (_buttonInfos.Count == 0)
            {
                _middleButton.gameObject.SetActive(false);
            }
            else
            {
                var peek = _buttonInfos[0];

                _buttonMapper.Setup(peek);
            }
        }

        private void OnLayersCountChanged(int newValue = 0)
        {
            _layerSlider.maxValue = _layersEvent.LayerCount - 1;
            _layerSlider.value = _layersEvent.CurrentLayer;

            bool shouldShow = _layersEvent.LayerCount > 1;

            _previewButton.gameObject.SetActive(shouldShow);
            _previewLabel.gameObject.SetActive(shouldShow);
        }

        private void OnCurrentLayerChanged(int newValue)
        {
            _layerSlider.value = _layersEvent.CurrentLayer;
        }

        private void OnSliderChanged(float value)
        {
            _layersEvent.CurrentLayer.Value = Mathf.RoundToInt(value);
            _layerSlider.value = Mathf.RoundToInt(value);
        }

        private void OnPreviewChanged(bool newValue)
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

        public void OnWeaponClick()
        {
            _shootEvent.Invoke(this);
        }

        public void OnMeleeClick()
        {
            _meleeEvent.Invoke(this);
        }
    }
}
