using Core.Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EventBinders
{
    [RequireComponent(typeof(Slider))]
    public class SliderDisplayer : MonoBehaviour
    {
        [SerializeField] private CappedIntEvent _source;
        
        private Slider _target;

        private void Start()
        {
            _target = GetComponent<Slider>();

            _source.OnValueChanged += OnValueChanged;
            _source.OnMaxValueChanged += OnMaxValueChanged;

            OnValueChanged();
            OnMaxValueChanged();
        }

        private void OnValueChanged(int value = 0)
        {
            _target.value = _source.Value;
        }

        private void OnMaxValueChanged(int value = 0)
        {
            _target.maxValue = _source.MaxValue;
        }

        private void OnDestroy()
        {
            _source.OnValueChanged -= OnValueChanged;
            _source.OnMaxValueChanged -= OnMaxValueChanged;
        }
    }
}
