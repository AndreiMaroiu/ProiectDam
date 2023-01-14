using UnityEngine;

namespace ModalWindows
{
    public partial class ModalWindow
    {
        public static void Show(IModalWindowData data)
        {
            SetDebug(data);
        }

        public static void ShowDialog(IModalWindowData data)
        {
            SetDebug(data, Time.timeScale);
        }

        public static void ShowDialog(IModalWindowData data, float timeScale)
        {
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
            SetDebug(new ModalWindowPages(pages, Time.timeScale), Time.timeScale);
        }

        private static void SetDebug(IModalWindowData data, float? lastTimeScale = null)
        {
#if UNITY_EDITOR
            if (Instance is not null)
            {
                Instance.SetWindow(data, lastTimeScale);
            }
            else
            {
                Debug.LogError("Null Modal window instace. Please make sure you add an GameObject with a ModalWindow component in your scene");
            }
#else
            Instance.SetWindow(data, lastTimeScale);
#endif
        }
    }
}
