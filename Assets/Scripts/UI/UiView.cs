using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UiView : MonoBehaviour
    {
        [SerializeField] 
        private GameObject UiSettingsWindow;

        [SerializeField] 
        private GameObject UiMainWindow;

        [SerializeField]
        private GameObject UiMainPanel;

        [SerializeField] 
        private GameObject UiGameOverText;

        [SerializeField] 
        private Slider Slider;
    
        [SerializeField] 
        private Text SliderValueText;

        [SerializeField] 
        private Text ScoreText;
    
        [SerializeField]
        private Text GameOverScoreValue;
    
        [SerializeField]
        private Text GameOverBestScoreValue;
    
        public event Action StartGameButtonPressed;
        public event Action<float> SettingsApplied;

        private float _ticksPerSecond = 1;
        private float _score;
        private const float SLIDER_STEP = 0.1f;

        void Awake()
        {
            UiGameOverText.SetActive(false);
            ScoreText.enabled = false;
            Slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public void OnStartGameButtonPressed()
        {
            UiMainPanel.SetActive(false);
            ScoreText.text = _score.ToString();
            ScoreText.enabled = true;
            StartGameButtonPressed?.Invoke();
        }
    
        public void OnSettingsButtonPressed()
        {
            UiMainWindow.SetActive(false);
            UiSettingsWindow.SetActive(true);
        
        }

        public void OnApplyButtonPressed()
        {
            UiSettingsWindow.SetActive(false);
            UiMainWindow.SetActive(true);
            SettingsApplied?.Invoke(_ticksPerSecond);
        }
    
        public void SetScore(float newScore)
        {
            _score = newScore;
            ScoreText.text = _score.ToString();
        }

        public void GameOver(float bestScore)
        {
            UiGameOverText.SetActive(true);
            GameOverScoreValue.text = _score.ToString();
            GameOverBestScoreValue.text = bestScore.ToString();
        
            Slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    
        private void OnSliderValueChanged(float value)
        {
            var sliderValue = value < SLIDER_STEP ? SLIDER_STEP : (int) (value / SLIDER_STEP) * SLIDER_STEP;
            Slider.value = (float) Math.Round(sliderValue, 1);
            SliderValueText.text = sliderValue.ToString();
            _ticksPerSecond = Slider.value;
        }
    }
}
