using System;
using UnityEngine;

namespace Services
{
    public class InputController : MonoBehaviour, IInputController
    {
        public event Action<Vector2Int> InputReceived;
        void Update()
        {
            var input = new Vector2Int();
        
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                input = new Vector2Int(0, 1);
            }
        
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                input = new Vector2Int(0, -1);
            }
        
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                input = new Vector2Int(-1, 0);
            }
        
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                input = new Vector2Int(1, 0);
            }
        
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                input = new Vector2Int(-1, 0);
            }

            if (!input.Equals(Vector2Int.zero))
            {
                InputReceived?.Invoke(input);
            }
        }
    }
}
