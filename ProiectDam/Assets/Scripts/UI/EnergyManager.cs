using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EnergyManager : MonoBehaviour
    {
        [SerializeField] private CappedIntEvent _energy;
        [SerializeField] private Slider _energySlider;
        [SerializeField] private Text _energyText;

        private void Start()
        {
            OnEnergyChange();
            OnMaxEnergyChange();
            _energy.OnValueChanged += OnEnergyChange;
            _energy.OnMaxValueChanged += OnMaxEnergyChange;
        }

        private void OnDestroy()
        {
            _energy.OnValueChanged -= OnEnergyChange;
            _energy.OnMaxValueChanged -= OnMaxEnergyChange;
        }

        private void OnEnergyChange()
        {
            _energySlider.value = _energy.Value;
            UpdateText();
        }

        private void OnMaxEnergyChange()
        {
            _energySlider.maxValue = _energy.MaxValue;
            UpdateText();
        }

        private void UpdateText()
        {
            _energyText.text = $"{_energy.Value.ToString()}/{_energy.MaxValue.ToString()}";
        }
    }
}
