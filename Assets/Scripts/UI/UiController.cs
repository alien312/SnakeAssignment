using System;
using Snake;

namespace UI
{
    public class UiController : IDisposable
    {
        public event Action StartGameButtonPressed;
        public event Action<float> ApplySettingsButtonPressed;

        private readonly ISnakeController _snakeController;
        private readonly UiView _uiView;

        public UiController(UiView uiView, ISnakeController snakeController)
        {
            _uiView = uiView;
            _uiView.StartGameButtonPressed += UiViewOnStartGameButtonPressed;
            _uiView.SettingsApplied += OnSettingsApplied;
            _snakeController = snakeController;
            snakeController.ScoreChanged += OnScoreChanged;
        
        }

        public void GameOver(float bestScore)
        {
            _uiView.GameOver(bestScore);
        }

        private void OnSettingsApplied(float value)
        {
            ApplySettingsButtonPressed?.Invoke(value);
        }

        private void UiViewOnStartGameButtonPressed()
        {
            StartGameButtonPressed?.Invoke();
        }

        private void OnScoreChanged(float newScore)
        {
            _uiView.SetScore(newScore);
        }

        public void Dispose()
        {
            _snakeController.ScoreChanged -= OnScoreChanged;
            _uiView.StartGameButtonPressed -= UiViewOnStartGameButtonPressed;
            _uiView.SettingsApplied -= OnSettingsApplied;
        }
    }
}
