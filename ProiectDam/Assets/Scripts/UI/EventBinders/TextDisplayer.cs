using Core.Events.Binding;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EventBinders
{
    public class TextDisplayer<TEvent, T> : MonoBehaviour where TEvent : IBindSource<T>
    {
        [SerializeField] private TEvent _source;
        [SerializeField] private Text _target;

        private void Start()
        {
            _source.Bindable.OnValueChanged += OnValueChanged;

            OnValueChanged();
        }

        private void OnValueChanged(T value = default)
        {
            _target.text = _source.ToString();
        }

        private void OnDestroy()
        {
            _source.Bindable.OnValueChanged -= OnValueChanged;
        }
    }
}
