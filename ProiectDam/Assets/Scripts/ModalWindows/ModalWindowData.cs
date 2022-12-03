using UnityEngine;
using UnityEngine.Events;

namespace ModalWindows
{
    public class ModalWindowData : IModalWindowData
    {
        public string Header { get; set; }
        public string Content { get; set; }
        public Sprite Image { get; set; }
        public string Footer { get; set; }
        public string OkText { get; set; } = "Ok";
        public string AlternativeText { get; set; }
        public string CloseText { get; set; } = "Close";
        public UnityAction OkAction { get; set; }
        public UnityAction CloseAction { get; set; }
        public UnityAction AlternativeAction { get; set; }
        public bool IsTransparent { get; set; } = true;
        public Color BackgroundColor { get; set; } = Color.black;
        public bool CanClose => true;
    }
}
