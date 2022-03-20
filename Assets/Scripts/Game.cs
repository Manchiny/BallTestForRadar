using System;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private LevelConstructor _levelConstructor;
    [SerializeField] private CameraFollow _cameraFollow;
    [SerializeField] private UIManager _uiManager;
    private int _maxLives = 3;
    public static Game Instance { get; private set; }

    private Vector3 _ballSpawnPosition = new Vector3(0, 0.51f, -49);
    private Ball _ball;

    private Dictionary<Type, Action> _ballCollisionsActions;
    private Dictionary<int, SpeedTreshold> _speedTresholds;
    private int _nextSpeedTresholdId;
    private int _nextSpeedTresholdScroes;

    private int _scores;
    public int Scores
    {
        get => _scores;
        private set
        {
            _scores = value;
            OnScoresChanged?.Invoke(_scores);
        }
    }
    private int _lives;
    private int Lives
    {
        get => _lives;
        set
        {
            _lives = value;
            OnLivesChanged?.Invoke(_lives);
        }
    }
    private Action OnScoresAdded;
    public Action<int> OnScoresChanged;
    public Action<int> OnLivesChanged;
    public Action OnGameOver;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            return;
        }

        Destroy(this);
    }
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _ballCollisionsActions = new Dictionary<Type, Action>();
        _ballCollisionsActions.Add(typeof(Bomb), DecreaseLives);
        _ballCollisionsActions.Add(typeof(Money), AddScroes);

        InitSpeedTresholds();
        InitLevel();

        _uiManager.Init(_ball);

        Scores = 0;
        Lives = _maxLives;
    }

    private void InitSpeedTresholds()
    {
        _speedTresholds = new Dictionary<int, SpeedTreshold>()
        {
            { 0, new SpeedTreshold(10, 1.5f)},
            { 1, new SpeedTreshold(25, 2f)},
            { 2, new SpeedTreshold(50, 3f)},
            { 3, new SpeedTreshold(100, 4f)},
        };
        _nextSpeedTresholdId = 0;
        _nextSpeedTresholdScroes = _speedTresholds[_nextSpeedTresholdId].Scores;

        OnScoresAdded += CheckScoresForNeedSpeedUp;
    }

    private void InitLevel()
    {
        _levelConstructor.Init();
        _ball = Instantiate(_ballPrefab, _ballSpawnPosition, Quaternion.identity);
        _ball.OnGameItemContact += OnBallContactItem;
        _ball.OnBorderContact += DecreaseLives;

        _cameraFollow.Init(_ball.transform);
    }

    private void OnBallContactItem(GameItem item)
    {
        item.Deactivate();
        _ballCollisionsActions[item.GetType()].Invoke();
    }

    private void AddScroes()
    {       
        Scores++;
        OnScoresAdded?.Invoke();
    }
    private void CheckScoresForNeedSpeedUp()
    {
        if (Scores >= _nextSpeedTresholdScroes)
        {
            _ball.SpeedUp(_speedTresholds[_nextSpeedTresholdId].Multiplier);
            _nextSpeedTresholdId += 1;
            if (_speedTresholds.ContainsKey(_nextSpeedTresholdId))
                _nextSpeedTresholdScroes = _speedTresholds[_nextSpeedTresholdId].Scores;
            else
                OnScoresAdded -= CheckScoresForNeedSpeedUp;
        }
    }
    private void DecreaseLives()
    {
        Lives--;
        if (Lives < 0)
        {
            _ball.StopBall();
            OnGameOver?.Invoke();
        }          
    }

    public void RestartGame()
    {
        _nextSpeedTresholdId = 0;
        _nextSpeedTresholdScroes = _speedTresholds[_nextSpeedTresholdId].Scores;

        Scores = 0;
        Lives = _maxLives;

        _levelConstructor.RestartLevel();
        _ball.Restart(_ballSpawnPosition);
    }
}

public struct SpeedTreshold
{
    public int Scores { get; private set; }
    public float Multiplier { get; private set; }
    public SpeedTreshold(int scores, float multiplier)
    {
        Scores = scores;
        Multiplier = multiplier;
    }
}
