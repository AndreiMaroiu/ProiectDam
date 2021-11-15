using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private CappedIntEvent _energy;
        [SerializeField] private Slider _energySlider;
        [SerializeField] private Text _energyText;

        [SerializeField] private CappedIntEvent _health;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Text _healthText;

        private static int max = 100;

        private void Start()
        {
            _energy.MaxValue = max;
            _health.MaxValue = max;

            OnEnergyChange();
            OnMaxEnergyChange();

            OnHealthChange();
            OnMaxHealthChange();

            _energy.OnValueChanged += OnEnergyChange;
            _energy.OnMaxValueChanged += OnMaxEnergyChange;

            _health.OnValueChanged += OnHealthChange;
            _health.OnMaxValueChanged += OnMaxHealthChange;
        }

        private void OnDestroy()
        {
            _energy.OnValueChanged -= OnEnergyChange;
            _energy.OnMaxValueChanged -= OnMaxEnergyChange;

            _health.OnValueChanged -= OnHealthChange;
            _health.OnMaxValueChanged -= OnMaxHealthChange;
        }

        private void OnEnergyChange()
        {
            _energySlider.value = _energy.Value;
            UpdateEnergyText();
        }

        private void OnMaxEnergyChange()
        {
            _energySlider.maxValue = _energy.MaxValue;
            UpdateEnergyText();
        }

        private void OnHealthChange()
        {
            _healthSlider.value = _health.Value;
            UpdateHealthText();
        }

        private void OnMaxHealthChange()
        {
            _healthSlider.maxValue = _health.MaxValue;
            UpdateHealthText();
        }

        private void UpdateEnergyText()
        {
            _energyText.text = $"{_energy.Value.ToString()}/{_energy.MaxValue.ToString()}";
        }

        private void UpdateHealthText()
        {
            _healthText.text = $"{_health.Value.ToString()}/{_health.MaxValue.ToString()}";
        }
    }
}
