using System;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    public class SnakeUnit : MonoBehaviour
    {
        public event Action<CollisionType> Collision;
    
        private SnakeUnit _nextUnit;
        public void Move(List<Vector2Int> moveSteps, ref int index)
        {
            var direction = moveSteps[index];
            index--;
            transform.position += new Vector3(direction.x, 0, direction.y);
            if (_nextUnit != null)
            {
                _nextUnit.Move(moveSteps, ref index);
            }
        }

        public void SetTail(SnakeUnit tail)
        {
            if (_nextUnit == null)
            {
                _nextUnit = tail;
            }
            else
            {
                _nextUnit.SetTail(tail);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case "Obstacle":
                    Collision?.Invoke(CollisionType.Obstacle);
                    break;
            }
        }
    }
}
