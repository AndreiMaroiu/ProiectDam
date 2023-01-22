using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public enum PanelType
    {
        Normal,
        Modal
    }

    public class PanelOptions
    {
        public PanelType PanelType { get; set; }
        public Func<PanelType, bool> CanClose { get; set; } = type => true;
        public Action OnClose { get; set; }
    }

    [CreateAssetMenu(fileName = "New Panel Stack", menuName = "Scriptables/Panel Stack")]
    public class PanelStack : ScriptableObject
    {
        private static readonly PanelOptions _default = new();

        private readonly Stack<(GameObject panel, PanelOptions options)> _panels = new();
        private float? _lastTimeScale;

        public bool CanClose => _panels.Count > 0;

        public int PanelsCount => _panels.Count;

        public void ClosePanel()
        {
            if (CanClose is false)
            {
                return;  
            }

            var (panel, options) = _panels.Pop();
            panel.SetActive(false);
            options.OnClose?.Invoke();

            if (_panels.Count > 0)
            {
                var (peek, _) = _panels.Peek();
                peek.SetActive(true);
            }

            if (_panels.Count is 0 && _lastTimeScale.HasValue)
            {
                Time.timeScale = _lastTimeScale.Value;
                _lastTimeScale = null;
            }
        }

        public void OpenPanel(GameObject panel, PanelOptions options = null, float? timeScale = null)
        {
            options ??= _default;

            if (_panels.Count > 0 && panel == _panels.Peek().panel)
            {
                return;
            }

            if (_panels.Count > 0)
            {
                var (lastPanel, peekOptions) = _panels.Peek();
                lastPanel.SetActive(!peekOptions.CanClose(options.PanelType));
            }

            _panels.Push((panel, options is null ? _default : options));
            panel.SetActive(true);

            if (_lastTimeScale is null && timeScale is not null)
            {
                _lastTimeScale = timeScale;
                Time.timeScale = 0;
            }
        }

        public void OpenDialog(GameObject panel, PanelOptions options = null) 
            => OpenPanel(panel, options is null ? _default : options, Time.timeScale);

        public void OnEnable()
        {
            _lastTimeScale = null;
            _panels.Clear();
            Debug.Log("panel stack cleared");
        }

        public void Clear()
        {
            OnEnable();
        }
    }
}
