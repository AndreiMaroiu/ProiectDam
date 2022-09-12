using System;
using UnityEngine;

namespace ModalWindows
{
    public interface IModalWindowData
    {
        public string Header { get; }
        public string Content { get; }
        public Sprite Image { get; }
        public string Footer { get; }
        public string OkText { get; }
        public string AlternativeText { get; }
        public string CloseText { get; }
        public Action OkAction { get; }
        public Action CloseAction { get; }
        public Action AlternativeAction { get; }
        public bool CanClose { get; }
        public bool IsTransparent { get; }
        public Color BackgroundColor { get; }
    }
}
