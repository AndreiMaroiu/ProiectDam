using Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ModalWindows;

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

        [SerializeField] private CappedIntEvent _bullets;
        [SerializeField] private Text _bulletsText;

        [SerializeField] private GameEvent _onPlayerDeath;

        private void Start()
        {
            OnMaxEnergyChange();
            OnEnergyChange();

            OnMaxHealthChange();
            OnHealthChange();

            OnMaxBulletsChange();
            OnBulletsChange();

            _energy.OnValueChanged += OnEnergyChange;
            _energy.OnMaxValueChanged += OnMaxEnergyChange;

            _health.OnValueChanged += OnHealthChange;
            _health.OnMaxValueChanged += OnMaxHealthChange;

            _bullets.OnValueChanged += OnBulletsChange;
            _bullets.OnMaxValueChanged += OnMaxBulletsChange;

            _onPlayerDeath.OnEvent += OnPlayerDeath;
        }

        private void OnDestroy()
        {
            _energy.OnValueChanged -= OnEnergyChange;
            _energy.OnMaxValueChanged -= OnMaxEnergyChange;

            _health.OnValueChanged -= OnHealthChange;
            _health.OnMaxValueChanged -= OnMaxHealthChange;

            _bullets.OnValueChanged -= OnBulletsChange;
            _bullets.OnMaxValueChanged -= OnMaxBulletsChange;

            _onPlayerDeath.OnEvent -= OnPlayerDeath;
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

        private void OnBulletsChange()
        {
            UpdateBulletsText();
        }

        private void OnMaxBulletsChange()
        {
            UpdateBulletsText();
        }

        private void UpdateEnergyText()
        {
            _energyText.text = $"{_energy.Value.ToString()}/{_energy.MaxValue.ToString()}";
        }

        private void UpdateHealthText()
        {
            _healthText.text = $"{_health.Value.ToString()}/{_health.MaxValue.ToString()}";
        }

        private void UpdateBulletsText()
        {
            _bulletsText.text = $"{_bullets.Value.ToString()}/{_bullets.MaxValue.ToString()}";
        }

        private void OnPlayerDeath()
        {
            float timeScale = Time.timeScale;
            Time.timeScale = 0.0f;
            ModalWindow.ShowSimpleDialog("You died!", () => { 
                Time.timeScale = timeScale;
                SceneManager.LoadScene(1);
            });
        }
    }
}