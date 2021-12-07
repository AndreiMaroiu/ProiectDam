using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class OptionsManager : MonoBehaviour
    {
        private const float MuteVolume = -80.00f;
        private const float MinVolume = -45.00f;
        private const float MaxVolume = 5.00f;
        
        [Header("Sliders")]
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _fxSlider;
        [SerializeField] private Slider _musicSlider;
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

            _masterSlider.value = PlayerPrefs.GetFloat("Master", MaxVolume);
            _fxSlider.value = PlayerPrefs.GetFloat("Sound Effects", MaxVolume);
            _musicSlider.value = PlayerPrefs.GetFloat("Music", MaxVolume);

            _version.text = "Version: " + Application.version;
        }

        private void OnMasterChanged(float value)
        {
            PlayerPrefs.SetFloat("Master", value);
            value = CheckMute(value);
            _volumes.SetFloat("Master", value);
        }

        private void OnFxChanged(float value)
        {
            PlayerPrefs.SetFloat("Sound Effects", value);
            value = CheckMute(value);
            _volumes.SetFloat("Sound Effects", value);
        }

        private void OnMusicChanged(float value)
        {
            PlayerPrefs.SetFloat("Music", value);
            value = CheckMute(value);
            _volumes.SetFloat("Music", value);
        }

        private void OnDestroy()
        {
            _masterSlider.onValueChanged.RemoveListener(OnMasterChanged);
            _fxSlider.onValueChanged.RemoveListener(OnFxChanged);
            _musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
        }

        private float CheckMute(float value)
        {
            if (value <= MinVolume)
            {
                return MuteVolume;
            }
            return value;
        }
    }
}
