using System.IO;
using Snake;
using UI;
using UnityEngine;

namespace Services
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] 
        private GameObject SnakePrefab;

        [SerializeField] 
        private GameObject FoodPrefab;

        [SerializeField] 
        private UiView UiView;
    
        private SnakeController _snakeController;
        private InputController _inputController;
        private FoodSpawnController _foodSpawnController;
        private UiController _uiController;

        private float _updateTimeout;
        private float _timer;
        private GameSettings _gameSettings;
        private string _path;
    
        void Awake()
        {
            _path = Application.dataPath + "//Settings//settings.json";
            if (File.Exists(_path))
            {
                var json = File.ReadAllText(_path);
                _gameSettings = JsonUtility.FromJson<GameSettings>(json);
                _gameSettings.FieldSize = new Vector2Int(50, 50);
            }
            else
            {
                _gameSettings = new GameSettings( 1, 0);
                var json = JsonUtility.ToJson(_gameSettings);
                File.WriteAllText(_path, json);
            }
        
            SetUp();
        }
    
        private void SetUp()
        {
            var fieldSize = _gameSettings.FieldSize;
            _updateTimeout = 1 / _gameSettings.TicksPerSeconds;
        
            var inputManagerGO = new GameObject("InputManager");
            _inputController = inputManagerGO.AddComponent<InputController>();

            var foodSpawnerGO = new GameObject("FoodSpawner");
            _foodSpawnController = foodSpawnerGO.AddComponent<FoodSpawnController>();
            if (_foodSpawnController != null)
            {
                _foodSpawnController.SetUp(FoodPrefab, fieldSize);
                _foodSpawnController.SpawnFood();
            }
        
            var snakeModel = new SnakeModel(fieldSize);
            var snakeObject = Instantiate(
                SnakePrefab, 
                new Vector3(fieldSize.x / 2, 0, fieldSize.y / 2),
                Quaternion.identity);
            var snakeView = snakeObject.GetComponent<SnakeView>();
            _snakeController = new SnakeController(snakeModel, snakeView, _inputController, _foodSpawnController);
            _snakeController.GameOver += SnakeControllerOnGameOver;
        
            _uiController = new UiController(UiView, _snakeController);
            _uiController.StartGameButtonPressed += OnStartGameButtonPressed;
            _uiController.ApplySettingsButtonPressed += OnApplySettingsButtonPressed;
        }
    
        private void SnakeControllerOnGameOver()
        {
            _snakeController.IsEnabled = false;
            _uiController.GameOver(_gameSettings.BestScore);
        }

        void Update()
        {
            if (_snakeController != null && _snakeController.IsEnabled)
            {
                _timer += Time.deltaTime;
                if (_timer >= _updateTimeout)
                {
                    _snakeController.Update();
                    _timer -= _updateTimeout;
                }
            }
        }
    
        private void OnApplySettingsButtonPressed(float value)
        {
            _gameSettings = new GameSettings(value, _gameSettings.BestScore);
        }

        private void OnStartGameButtonPressed()
        {
            _snakeController.IsEnabled = true;
        }

        private void OnDestroy()
        {
            if (_gameSettings.BestScore < _snakeController.Score)
            {
                _gameSettings.BestScore = _snakeController.Score;
            } 
            var json = JsonUtility.ToJson(_gameSettings);
            File.WriteAllText(_path, json);
        
            _snakeController?.Dispose();
            _uiController?.Dispose();
        }
    }
}
