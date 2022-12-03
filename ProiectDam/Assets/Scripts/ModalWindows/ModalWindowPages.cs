using System;
using UnityEngine;
using UnityEngine.Events;

namespace ModalWindows
{
    public class ModalWindowPages : IModalWindowData
    {
        private readonly ModalWindowPageData[] _pages;
        private readonly float _lastTimeScale;
        private int _currentIndex;
        private int _lastIndex;

        public ModalWindowPages(ModalWindowPageData[] pages, float timeScale)
        {
#if UNITY_EDITOR
            if (pages is null || pages.Length == 0)
            {
                throw new Exception("Need at least one page!");
            }
#endif
            _pages = pages;
            _lastIndex = pages.Length - 1;
            _currentIndex = 0;
            _lastTimeScale = timeScale;

            Current = _pages[0];
            CloseAction = Next;
            OkAction = Previous;
        }

        public ModalWindowPageData Current { get; private set; }
        public string Header => Current?.Header;
        public string Content => Current?.Content;
        public Sprite Image => Current?.Image;
        public string Footer => Current?.Footer;
        public string OkText => (_currentIndex > 0) ? "Previous" : null;
        public string AlternativeText => null;
        public string CloseText => (_currentIndex == _lastIndex) ? "Close" : "Next";
        public UnityAction OkAction { get; }
        public UnityAction CloseAction { get; }
        public UnityAction AlternativeAction => null;
        public bool CanClose { get; private set; } = false;
        public bool IsTransparent { get; set; } = true;
        public Color BackgroundColor { get; set; } = Color.black;

        private void Next()
        {
            _currentIndex++;

            UpdateModal();
        }

        private void Previous()
        {
            _currentIndex--;

            UpdateModal();
        }

        private void UpdateModal()
        {
            if (_currentIndex > _lastIndex)
            {
                Current = null;
                CanClose = true;
            }
            else
            {
                Current = _pages[_currentIndex];
                CanClose = false;
            }

            ModalWindow.ShowDialog(this, _lastTimeScale);
        }
    }
}
