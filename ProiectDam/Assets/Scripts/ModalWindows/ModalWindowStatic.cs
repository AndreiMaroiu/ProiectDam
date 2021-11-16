using System;
using UnityEngine;

namespace ModalWindows
{
    public partial class ModalWindow
    {
        public static void Show(string header, string content, Sprite image, string footer,
            Action okAction = null, Action closeAction = null, Action alternativeAction = null)
        {
            SetDebug(header, content, image, footer, okAction, closeAction, alternativeAction);
        }

        public static void ShowMessage(string message)
        {
            SetDebug(message, null, null, null);
        }

        public static void ShowSimpleDialog(string message, Action closeAction)
        {
            SetDebug(message, null, null, null, null, closeAction);
        }

        private static void SetDebug(string header, string content, Sprite image, string footer,
            Action okAction = null, Action closeAction = null, Action alternativeAction = null)
        {
            if (!ReferenceEquals(Instance, null))
            {
                Instance.SetWindow(header, content, image, footer, okAction, closeAction, alternativeAction);
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
