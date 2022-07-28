using System;
using UnityEngine;

namespace ModalWindows
{
    public partial class ModalWindow
    {
        public static void Show(ModalWindowData data)
        {
            SetDebug(data);
        }

        public static void ShowDialog(float timeScale, ModalWindowData data)
        {
            Time.timeScale = 0;
            SetDebug(data, timeScale);
        }

        public static void ShowMessage(string message)
        {
            SetDebug(new ModalWindowData()
            {
                Content = message
            });
        }

        private static void SetDebug(ModalWindowData data, float? lastTimeScale = null)
        {
            if (!ReferenceEquals(Instance, null))
            {
                Instance.SetWindow(data);
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError("Null Modal window instace. Please make sure you add an GameObject with a ModalWindow component in your scene");
            }
#endif
        }
    }
}
