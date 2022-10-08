using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class RefreshRateSetter : MonoBehaviour
    {
#if PLATFORM_ANDROID && !UNITY_EDITOR
        private void Awake()
        {
            Resolution[] resolutions = Screen.resolutions;

            if (resolutions is null || resolutions.Length < 1)
            {
                return;
            }

            Resolution target = resolutions[0];
            Application.targetFrameRate = target.refreshRate;
            Debug.Log($"target fps: {Application.targetFrameRate.ToString()}");
        }
#endif
    }
}
