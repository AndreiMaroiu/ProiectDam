using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class VibrationManager
    {
        public static VibrationManager Instance { get; } = new VibrationManager();

        private const string SettingPath = "CanVibrate";
        private bool _canVibrate;

        public VibrationManager()
        {
            // by default setting is set to true
            _canVibrate = PlayerPrefs.GetInt(SettingPath, defaultValue: 1) == 1 ? true : false; 
        }

        public bool CanVibrate
        {
            get => _canVibrate;
            set
            {
                _canVibrate = value;

                int canVibrateInt = value ? 1 : 0;

                PlayerPrefs.SetInt(SettingPath, canVibrateInt);
            }
        }

        public bool TryVibrate()
        {
#if !PLATFORM_WEBGL
            if (CanVibrate)
            {
                Handheld.Vibrate();
            }

            return CanVibrate;
#else
            return false;
#endif
        }
    }
}
