using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private readonly Stack<(GameObject panel, PanelOptions options, float? timeScale)> _panels = new();

        public bool CanClose => _panels.Count > 0;

        public int PanelsCount => _panels.Count;

        public void ClosePanel()
        {
            if (CanClose is false)
            {
                return;
            }

            var (panel, options, timeScale) = _panels.Pop();
            panel.SetActive(false);
            options.OnClose?.Invoke();

            if (timeScale.HasValue)
            {
                Time.timeScale = timeScale.Value;
            }

            if (_panels.Count > 0)
            {
                var (peek, _, _) = _panels.Peek();
                peek.SetActive(true);

                if (timeScale.HasValue)
                {
                    Time.timeScale = timeScale.Value;
                }
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
                var (lastPanel, peekOptions, _) = _panels.Peek();
                lastPanel.SetActive(!peekOptions.CanClose(options.PanelType));
            }

            _panels.Push((panel, options is null ? _default : options, timeScale));
            panel.SetActive(true);

            if (timeScale is not null)
            {
                Time.timeScale = 0;
            }
        }

        public void OpenDialog(GameObject panel, PanelOptions options = null)
            => OpenPanel(panel, options is null ? _default : options, Time.timeScale);

        public void OnEnable()
        {
            _panels.Clear();
            Debug.Log("panel stack cleared");

            SceneManager.sceneUnloaded += SceneChanged;
        }

        public void Clear()
        {
            OnEnable();
        }

        public void CloseAll()
        {
            while (CanClose)
            {
                ClosePanel();
            }
        }

        private void OnDisable()
        {
            SceneManager.sceneUnloaded -= SceneChanged;
        }

        private void SceneChanged(Scene arg0)
        {
            Clear();
        }
    }
}
