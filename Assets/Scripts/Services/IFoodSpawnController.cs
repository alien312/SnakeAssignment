using UnityEngine;

namespace Services
{
    public interface IFoodSpawnController
    {
        Vector2Int? FoodPosition { get; }
        void SpawnFood();
    }
}