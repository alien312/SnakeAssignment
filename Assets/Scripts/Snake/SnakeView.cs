using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Snake
{
    public class SnakeView : MonoBehaviour
    {
        [SerializeField] 
        private SnakeUnit SnakeHead;
        [SerializeField] 
        private GameObject SnakeBody;
        [SerializeField] 
        private GameObject SnakeBodyPrefab;

        public event Action<CollisionType> CollisionRecived;
    
        private Transform _tailPivot;
        private SnakeUnit _tailObject;
        private int _bodySize;

        private readonly Queue<Vector2Int> _moveSteps = new Queue<Vector2Int>();

        private void Awake()
        {
            _tailPivot = SnakeHead.transform;
            _tailObject = SnakeHead;
            SnakeHead.Collision += OnCollisionReceived;
        }

        public void Move(Vector2Int moveDirection)
        {
            _moveSteps.Enqueue(moveDirection);
            if (_moveSteps.Count > _bodySize + 1)
            {
                _moveSteps.Dequeue();
            }

            var index = _moveSteps.Count - 1;
            SnakeHead.Move(_moveSteps.ToList(), ref index);
        }

        public void IncreaseBodySize()
        {
            var tailDirection = _moveSteps.Peek();
            var tailPosition = _tailPivot.transform. position - new Vector3(tailDirection.x, 0, tailDirection.y);
            var tailGO = Instantiate(SnakeBodyPrefab, tailPosition, Quaternion.identity, SnakeBody.transform);
            _tailObject = tailGO.GetComponent<SnakeUnit>();
            _tailPivot = tailGO.transform;
            SnakeHead.SetTail(_tailObject);
            _bodySize++;
        }

        private void OnCollisionReceived(CollisionType collisionType)
        {
            CollisionRecived?.Invoke(collisionType);
        }

        private void OnDestroy()
        {
            if (SnakeHead != null)
            {
                SnakeHead.Collision -= OnCollisionReceived;
            }
        }
    }
}
