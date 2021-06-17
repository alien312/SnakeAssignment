using System;
using UnityEngine;

namespace Services
{
    [Serializable]
    public struct GameSettings 
    {
        [NonSerialized] public Vector2Int FieldSize;
        public float TicksPerSeconds;
        public float BestScore;

        public GameSettings(float ticksPerSeconds, float bestScore)
        {
            FieldSize = new Vector2Int(50, 50);
            TicksPerSeconds = ticksPerSeconds;
            BestScore = bestScore;
        }
    }
}