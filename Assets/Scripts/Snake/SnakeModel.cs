using System;
using UnityEngine;

namespace Snake
{
    public class SnakeModel
    {
        public event Action FoodReached; 
        public event Action<Vector2Int> PositionChanged;
        public event Action BoundariesReached;
    
        private Vector2Int _fieldSize;
        private Vector2Int _position;
        private Vector2Int _direction = Vector2Int.down;
        private Vector2Int _foodPosition;
    
        private float _timer;
        public SnakeModel(Vector2Int fieldSize)
        {
            _fieldSize = fieldSize;
            _position = new Vector2Int(fieldSize.x / 2, fieldSize.y / 2);
        }

        public void SetFoodPosition(Vector2Int? foodPosition)
        {
            if (foodPosition != null)
            {
                _foodPosition = foodPosition.Value;
            }
            else
            {
                Debug.Log("Food position is null");
            }
        }
    
        public void HandleInput(Vector2Int input)
        {
            if (input.Equals(-_direction))
            {
                return;
            }
        
            _direction = input;
            _position += input;
            PositionChanged?.Invoke(_direction);
        
            if (_position.x >= _fieldSize.x || _position.x <= 0 || _position.y >= _fieldSize.y || _position.y <= 0)
            {
                BoundariesReached?.Invoke();
                return;
            }

            if (_position.Equals(_foodPosition))
            {
                FoodReached?.Invoke();
            }
        }

        public void Update()
        {
            HandleInput(_direction);
        }
    }
}
