using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    [CreateAssetMenu(fileName = "New Panel Stack", menuName = "Scriptables/Panel Stack")]
    public class PanelStack : ScriptableObject
    {
        private readonly Stack<GameObject> _panels = new();
        private float? _lastTimeScale;

        public void ClosePanel()
        {
            if (_panels.Count is 0)
            {
                return;
            }

            GameObject panel = _panels.Pop();
            panel.SetActive(false);

            if (_panels.Count > 0)
            {
                GameObject peek = _panels.Peek();
                peek.SetActive(true);
            }

            if (_panels.Count is 0 && _lastTimeScale.HasValue)
            {
                Time.timeScale = _lastTimeScale.Value;
                _lastTimeScale = null;
            }
        }

        public void OpenPanel(GameObject panel, float? timeScale = null)
        {
            if (_panels.Count > 0 && panel == _panels.Peek())
            {
                return;
            }

            if (_panels.Count > 0)
            {
                GameObject lastPanel = _panels.Peek();
                lastPanel.SetActive(false);
            }

            _panels.Push(panel);
            panel.SetActive(true);

            if (_lastTimeScale is null && timeScale is not null)
            {
                _lastTimeScale = timeScale;
                Time.timeScale = 0;
            }
        }

        public void OpenDialog(GameObject panel) => OpenPanel(panel, Time.timeScale);

        public void OnEnable()
        {
            _lastTimeScale = null;
            _panels.Clear();
            Debug.Log("panel stack cleared");
        }
    }
}
