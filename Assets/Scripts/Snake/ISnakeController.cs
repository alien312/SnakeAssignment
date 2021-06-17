using System;

namespace Snake
{
    public interface ISnakeController
    {
        event Action<float> ScoreChanged;
    }
}