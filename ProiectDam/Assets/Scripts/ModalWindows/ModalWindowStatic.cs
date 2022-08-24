using System;
using UnityEngine;

namespace ModalWindows
{
    public partial class ModalWindow
    {
        public static void Show(IModalWindowData data)
        {
            SetDebug(data);
        }

        public static void ShowDialog(ModalWindowData data)
        {
            float timeScale = Time.timeScale;
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

        public static void ShowPages(params ModalWindowPageData[] pages)
        {
            SetDebug(new ModalWindowPages(pages), Time.timeScale);
        }

        private static void SetDebug(IModalWindowData data, float? lastTimeScale = null)
        {
            if (!ReferenceEquals(Instance, null))
            {
                Instance.SetWindow(data, lastTimeScale);
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
