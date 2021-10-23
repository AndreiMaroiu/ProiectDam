using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] private Text _layerCount;
        [SerializeField] private Text _currentLayer;
        [Header("Events")]
        [SerializeField] private IntEvent _layersCountEvent;
        [SerializeField] private IntEvent _currentLayerEvent;

        private void Start()
        {
            _layersCountEvent.OnValueChanged += OnLayersCountChanged;
            _currentLayerEvent.OnValueChanged += OnCurrentLayerChanged;

            OnLayersCountChanged();
            OnCurrentLayerChanged();
        }

        private void OnLayersCountChanged()
        {
            _layerCount.text = _layersCountEvent.Value.ToString();
        }

        private void OnCurrentLayerChanged()
        {
            _currentLayer.text = _currentLayerEvent.Value.ToString();
        }
    }
}
