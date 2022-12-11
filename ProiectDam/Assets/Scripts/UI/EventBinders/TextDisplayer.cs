using Core.Events.Binding;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EventBinders
{
    [RequireComponent(typeof(Text))]   
    public class TextDisplayer<TEvent, T> : MonoBehaviour where TEvent : IBindSource<T>
    {
        [SerializeField] private TEvent _source;

        private Text _target;

        private void Start()
        {
            _target = GetComponent<Text>();
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
