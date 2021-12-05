using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class VolumeManager : MonoBehaviour
    {
        private const float MinVolume = 0.0001f;
        private const float MaxVolume = 5.00f;

        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _fxSlider;
        [SerializeField] private Slider _musicSlider;

        [SerializeField] private AudioMixer _volumes;

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

            _volumes.SetFloat("Master", ToVolume(_masterSlider.value));
            _volumes.SetFloat("Sound Effects", ToVolume(_fxSlider.value));
            _volumes.SetFloat("Music", ToVolume(_musicSlider.value));
        }

        private void OnMasterChanged(float value)
        {
            _volumes.SetFloat("Master", ToVolume(value));
            PlayerPrefs.SetFloat("Master", value);
        }

        private void OnFxChanged(float value)
        {
            _volumes.SetFloat("Sound Effects", ToVolume(value));
            PlayerPrefs.SetFloat("Sound Effects", value);

        }

        private void OnMusicChanged(float value)
        {
            _volumes.SetFloat("Music", ToVolume(value));
            PlayerPrefs.SetFloat("Music", value);
        }

        private float ToVolume(float value)
        {
            return Mathf.Log10(value) * 20;
        }

        private void OnDestroy()
        {
            _masterSlider.onValueChanged.RemoveListener(OnMasterChanged);
            _fxSlider.onValueChanged.RemoveListener(OnFxChanged);
            _musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
        }
    }
}
