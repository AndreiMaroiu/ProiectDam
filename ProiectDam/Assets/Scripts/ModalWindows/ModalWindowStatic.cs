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

        public static void ShowMessage(string message)
        {
            SetDebug(new ModalWindowData()
            {
                Content = message
            });
        }

        public static void ShowSimpleDialog(string message, Action closeAction)
        {
            SetDebug(new ModalWindowData()
            {
                Content = message,
                CloseAction = closeAction
            });
        }

        private static void SetDebug(ModalWindowData data)
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
