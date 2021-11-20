using System;
using UnityEngine;

namespace ModalWindows
{
    public class ModalWindowData
    {
        public string Header { get; set; }
        public string Content { get; set; }
        public Sprite Image { get; set; }
        public string Footer { get; set; }
        public string OkText { get; set; } = "Ok";
        public string AlternativeText { get; set; }
        public string CloseText { get; set; } = "Close";
        public Action OkAction { get; set; }
        public Action CloseAction { get; set; }
        public Action AlternativeAction { get; set; }

    }
}
