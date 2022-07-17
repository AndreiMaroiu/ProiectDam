using Core.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ModalWindows;
using Utilities;
using GameStatistics;

namespace UI
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private CappedIntEvent _energy;
        [SerializeField] private CappedIntEvent _health;
        [SerializeField] private CappedIntEvent _bullets;
        [SerializeField] private GameEvent _onPlayerDeath;
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

            _onPlayerDeath.OnEvent += OnPlayerDeath;
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

            _onPlayerDeath.OnEvent -= OnPlayerDeath;
            _scoreEvent.OnValueChanged -= OnScoreChange;
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

        private void OnPlayerDeath()
        {
            Statistics data = StatisticsManager.Intance.LoadStats();

            data.AddLoss();

            StatisticsManager.Intance.Save(data);

            ModalWindow.Show(new ModalWindowData()
            {
                Header = "You Died!",
                Content = "Score: " + _scoreEvent.Value.ToString(),
                CloseText = "Main Menu",
                CloseAction = () => SceneManager.LoadScene(Scenes.MainMenu),
                OkText = "Play Again",
                OkAction = () => SceneManager.LoadScene(Scenes.MainScene)
            });
        }

        private void OnScoreChange()
        {
            _scoreText.text = _scoreEvent.Value.ToString();
        }
    }
}
