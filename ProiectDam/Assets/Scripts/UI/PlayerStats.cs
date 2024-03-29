using Core.Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private CappedIntEvent _energy;
        [SerializeField] private CappedIntEvent _health;
        [SerializeField] private CappedIntEvent _bullets;
        [SerializeField] private IntEvent _scoreEvent;
        [Header("Sliders")]
        [SerializeField] private Slider _energySlider;
        [SerializeField] private Slider _healthSlider;
        [Header("Texts")]
        [SerializeField] private Text _energyText;
        [SerializeField] private Text _healthText;
        [SerializeField] private Text _bulletsText;
        [SerializeField] private Text _scoreText;


        private void Start()
        {
            OnMaxEnergyChange();
            OnEnergyChange();

            OnMaxHealthChange();
            OnHealthChange();

            OnMaxBulletsChange();
            OnBulletsChange();

            OnScoreChange();

            _energy.OnValueChanged += OnEnergyChange;
            _energy.OnMaxValueChanged += OnMaxEnergyChange;

            _health.OnValueChanged += OnHealthChange;
            _health.OnMaxValueChanged += OnMaxHealthChange;

            _bullets.OnValueChanged += OnBulletsChange;
            _bullets.OnMaxValueChanged += OnMaxBulletsChange;

            _scoreEvent.OnValueChanged += OnScoreChange;
        }

        private void OnDestroy()
        {
            _energy.OnValueChanged -= OnEnergyChange;
            _energy.OnMaxValueChanged -= OnMaxEnergyChange;

            _health.OnValueChanged -= OnHealthChange;
            _health.OnMaxValueChanged -= OnMaxHealthChange;

            _bullets.OnValueChanged -= OnBulletsChange;
            _bullets.OnMaxValueChanged -= OnMaxBulletsChange;

            _scoreEvent.OnValueChanged -= OnScoreChange;
        }

        private void OnEnergyChange(int value = 0)
        {
            _energySlider.value = _energy.Value;
            UpdateEnergyText();
        }

        private void OnMaxEnergyChange(int value = 0)
        {
            _energySlider.maxValue = _energy.MaxValue;
            UpdateEnergyText();
        }

        private void OnHealthChange(int value = 0)
        {
            _healthSlider.value = _health.Value;
            UpdateHealthText();
        }

        private void OnMaxHealthChange(int value = 0)
        {
            _healthSlider.maxValue = _health.MaxValue;
            UpdateHealthText();
        }

        private void OnBulletsChange(int value = 0)
        {
            UpdateBulletsText();
        }

        private void OnMaxBulletsChange(int value = 0)
        {
            UpdateBulletsText();
        }

        private void UpdateEnergyText()
        {
            _energyText.text = _energy.ToString();
        }

        private void UpdateHealthText()
        {
            _healthText.text = _health.ToString();
        }

        private void UpdateBulletsText()
        {
            _bulletsText.text = _bullets.ToString();
        }

        private void OnScoreChange(int score = 0)
        {
            _scoreText.text = _scoreEvent.Value.ToString();
        }
    }
}
