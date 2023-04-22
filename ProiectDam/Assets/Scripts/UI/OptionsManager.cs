using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Utilities;
using static UnityEngine.GraphicsBuffer;

namespace UI
{
    public class OptionsManager : MonoBehaviour
    {
        private const float MuteVolume = -80.00f;
        private const float MinVolume = -45.00f;
        private const float MaxVolume = 5.00f;

        private const string Sfx = "Sound Effects";
        private const string Master = "Master";
        private const string Music = "Music";

        [Header("Sliders")]
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _fxSlider;
        [SerializeField] private Slider _musicSlider;
        [Header("Toggles")]
        [SerializeField] private Toggle _vibrationToggle;
        [Header("Dropdown")]
        [SerializeField] private Dropdown _refreshRateDropdown;
        [Header("Mixer")]
        [SerializeField] private AudioMixer _volumes;
        [Header("Version")]
        [SerializeField] private Text _version;

        private void Awake()
        {
            _masterSlider.minValue = MinVolume;
            _fxSlider.minValue = MinVolume;
            _musicSlider.minValue = MinVolume;

            _masterSlider.maxValue = MaxVolume;
            _fxSlider.maxValue = MaxVolume;
            _musicSlider.maxValue = MaxVolume;

            _masterSlider.onValueChanged.AddListener(OnMasterChanged);
            _fxSlider.onValueChanged.AddListener(OnFxChanged);
            _musicSlider.onValueChanged.AddListener(OnMusicChanged);
            _vibrationToggle.onValueChanged.AddListener(OnVibrationChanged);

            _masterSlider.value = PlayerPrefs.GetFloat(Master, MaxVolume);
            _fxSlider.value = PlayerPrefs.GetFloat(Sfx, MaxVolume);
            _musicSlider.value = PlayerPrefs.GetFloat(Music, MaxVolume);
            _vibrationToggle.isOn = VibrationManager.Instance.CanVibrate;

            _version.text = "Version: " + Application.version;

            _refreshRateDropdown.options = CreateOptions();
            _refreshRateDropdown.onValueChanged.AddListener(OnRefreshRateChanged);
            _refreshRateDropdown.value = _refreshRateDropdown.options.Count - 1;
        }

        private void OnDestroy()
        {
            _masterSlider.onValueChanged.RemoveListener(OnMasterChanged);
            _fxSlider.onValueChanged.RemoveListener(OnFxChanged);
            _musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
            _vibrationToggle.onValueChanged.RemoveListener(OnVibrationChanged);
            _refreshRateDropdown.onValueChanged.RemoveListener(OnRefreshRateChanged);
        }

        private void OnMasterChanged(float value) => SetVolumeValue(Master, value);

        private void OnFxChanged(float value) => SetVolumeValue(Sfx, value);

        private void OnMusicChanged(float value) => SetVolumeValue(Music, value);

        private void SetVolumeValue(string volumeType, float value)
        {
            PlayerPrefs.SetFloat(volumeType, value);
            value = CheckMute(value);
            _volumes.SetFloat(volumeType, value);
        }

        private void OnVibrationChanged(bool toggled) => VibrationManager.Instance.CanVibrate = toggled;

        private float CheckMute(float value)
        {
            if (value <= MinVolume)
            {
                return MuteVolume;
            }
            return value;
        }

        private void OnRefreshRateChanged(int index)
        {
            Application.targetFrameRate = (index + 1) * 30;
        }

        private static List<Dropdown.OptionData> CreateOptions()
        {
            List<Dropdown.OptionData> options = new();
            Resolution resolution = Screen.currentResolution;
            int i = 30;

            do
            {
                options.Add(new Dropdown.OptionData($"{i.ToString()} Hz"));
                i += 30;
            } while (i <= resolution.refreshRate);

            return options;
        }
    }
}
