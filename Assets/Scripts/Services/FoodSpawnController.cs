using UnityEngine;
using Random = UnityEngine.Random;

namespace Services
{
    public class FoodSpawnController : MonoBehaviour, IFoodSpawnController
    {
        public Vector2Int? FoodPosition { get; private set; }
    
        private GameObject _foodPrefab;
        private Vector2Int _fieldSize;

        private GameObject _foodObject;
    
        private const int MIN_SPAWN_X = 1;
        private const int MIN_SPAWN_Y = 1;
        private const int BOUNDARIES_INDENTION = 1;

        public void SetUp(GameObject foodPrefab, Vector2Int fieldSize)
        {
            _foodPrefab = foodPrefab;
            if (_foodPrefab == null)
            {
                Debug.Log("FoodObject is not set");
                FoodPosition = null;
            }
        
            _fieldSize = fieldSize;
        }
    
        public void SpawnFood()
        {
            if (_foodObject == null)
            {
                _foodObject = Instantiate(_foodPrefab);
                _foodObject.name = "Food";
            }

            var foodPositionX = Random.Range(MIN_SPAWN_X + BOUNDARIES_INDENTION, _fieldSize.x - BOUNDARIES_INDENTION);
            var foodPositionY = Random.Range(MIN_SPAWN_Y + BOUNDARIES_INDENTION, _fieldSize.y - BOUNDARIES_INDENTION);
            _foodObject.transform.position = new Vector3(foodPositionX, 1, foodPositionY);
            FoodPosition = new Vector2Int(foodPositionX, foodPositionY);
        }
    }
}
