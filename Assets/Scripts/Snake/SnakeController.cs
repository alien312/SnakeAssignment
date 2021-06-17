using System;
using Services;
using UnityEngine;

namespace Snake
{
    public class SnakeController : ISnakeController, IDisposable
    {
        public event Action<float> ScoreChanged;
        public event Action GameOver;

        public bool IsEnabled { get; set; }

        public float Score => _score;

        private readonly IInputController _inputController;
        private readonly IFoodSpawnController _foodSpawnController;
        private readonly SnakeModel _snakeModel;
        private readonly SnakeView _snakeView;

        private float _score;
    
        public SnakeController(SnakeModel snakeModel, SnakeView snakeView, IInputController inputController, IFoodSpawnController foodSpawnController)
        {
            _snakeModel = snakeModel;
            _snakeModel.FoodReached += OnFoodReached;
            _snakeModel.PositionChanged += OnPositionChanged;
            _snakeModel.BoundariesReached += OnBoundariesReached;
            _snakeView = snakeView;
            _snakeView.CollisionRecived += OnCollisionReceived;
        
            _inputController = inputController;
            _foodSpawnController = foodSpawnController;

            if (_foodSpawnController != null)
            {
                _snakeModel.SetFoodPosition(_foodSpawnController.FoodPosition);
            }
            else
            {
                Debug.Log($"Component of type {typeof(IFoodSpawnController)} were not found");
            }
        
            if (_inputController == null)
            {
                Debug.Log($"Component of type {typeof(IInputController)} were not found");
                return;
            }
            _inputController.InputReceived += OnInputReceived;
        }

        private void OnBoundariesReached()
        {
            GameOver?.Invoke();
        }

        public void Update()
        {
            _snakeModel.Update();
        }
    
        private void OnFoodReached()
        {
            _score += 100;
            ScoreChanged?.Invoke(_score);
            _foodSpawnController.SpawnFood();
            _snakeModel.SetFoodPosition(_foodSpawnController.FoodPosition);
            _snakeView.IncreaseBodySize();
        }
    
        private void OnCollisionReceived(CollisionType collisionType)
        {
            if (collisionType.Equals(CollisionType.Obstacle))
            {
                GameOver?.Invoke();
            }
        }
    
        private void OnPositionChanged(Vector2Int moveDirection)
        {
            _snakeView.Move(moveDirection);
        }
    
        private void OnInputReceived(Vector2Int input)
        {
            _snakeModel.HandleInput(input);
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _snakeModel.FoodReached -= OnFoodReached;
            _snakeModel.BoundariesReached -= OnBoundariesReached;
            _snakeModel.PositionChanged -= OnPositionChanged;
            _snakeView.CollisionRecived -= OnCollisionReceived;

            if (_inputController == null)
            {
                return;
            }

            _inputController.InputReceived -= OnInputReceived;
        }

        #endregion
    }
}
