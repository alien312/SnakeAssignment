using System;
using UnityEngine;

namespace Services
{
    public interface IInputController
    {
        event Action<Vector2Int> InputReceived;
    }
}