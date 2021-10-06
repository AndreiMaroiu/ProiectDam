using System;
using UnityEngine;

namespace ModalWindows
{
    public partial class ModalWindow
    {
        public static void Show(string header, string content, Sprite image, string footer,
            Action okAction = null, Action closeAction = null, Action alternativeAction = null)
        {
#if UNITY_EDITOR
            SetDebug(header, content, image, footer, okAction, closeAction, alternativeAction);
#else
            if (!ReferenceEquals(Instance, null))
            {
                Instance.SetWindow(header, content, image, footer, okAction, closeAction, alternativeAction);
            }
#endif
        }

        public static void ShowMessage(string message)
        {
#if UNITY_EDITOR
            SetDebug(message, null, null, null, null, null, null);
#else
            if (!ReferenceEquals(Instance, null))
            {
                Instance.SetWindow(message, null, null, null, null, null, null);
            }
#endif
        }

#if UNITY_EDITOR
        private static void SetDebug(string header, string content, Sprite image, string footer,
            Action okAction = null, Action closeAction = null, Action alternativeAction = null)
        {
            if (!ReferenceEquals(Instance, null))
            {
                Instance.SetWindow(header, content, image, footer, okAction, closeAction, alternativeAction);
            }
            else
            {
                Debug.LogError("Null Modal window instace. Please make sure you add an GameObject with a ModalWindow component in your scene");
            }
        }
#endif
    }
}
