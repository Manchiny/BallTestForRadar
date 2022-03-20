using System.Collections.Generic;
using UnityEngine;

public class LevelConstructor : MonoBehaviour
{
    [SerializeField] private GameObject _roadPrefab;
    [SerializeField] private Transform _roadPoolTransform;

    private int _maxRoadLength = 5;

    private Road _lastRoad; 
    private Queue<Road> _roadPool = new Queue<Road>();

    private int _poolCapacity = 25;
    public Queue<Road> CurrentRoad { get; private set; } = new Queue<Road>();
    private int _currentRoadCount = 0;

    public void Init()
    {
        CreatePool();
        CreateTrack();
    }

    /// <summary>
    /// Помещает участок дороги на сцену
    /// </summary>
    private void AddNextRoad()
    {
        Vector3 position = new Vector3();

        if (_lastRoad != null)
        {
            position = _lastRoad.transform.position + new Vector3(0, 0, 100f);
        }
        else
        {
            position = new Vector3(0, -0.5f, 0);
        }

        Road nextRoad = _roadPool.Dequeue();

        nextRoad.transform.position = position;
        nextRoad.gameObject.SetActive(true);
        nextRoad.ShowItems();

        _lastRoad = nextRoad;
        CurrentRoad.Enqueue(nextRoad);
        _currentRoadCount++;

        if (_currentRoadCount > _maxRoadLength)
        {
            RemoveRoad();
        }
    }


    /// <summary>
    /// Создает и конфигурирвует пул из участков дороги
    /// </summary>
    private void CreatePool()
    {
        for (int i = 0; i < _poolCapacity; i++)
        {
            Road newRoad = Instantiate(_roadPrefab, _roadPoolTransform.position, Quaternion.identity).GetComponent<Road>();

            _roadPool.Enqueue(newRoad);

            newRoad.EndRoadChecker.OnRoadEnded += AddNextRoad;
            newRoad.CreateRandomItems();
            newRoad.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Первоначальное размещение участков дороги на сцену
    /// </summary>
    private void CreateTrack()
    {
        for (int i = 0; i < _maxRoadLength - 1; i++)
        {
            AddNextRoad();
        }
    }

    /// <summary>
    /// Выключает наиболее рано размещенный на сцене участок дороги
    /// </summary>
    private void RemoveRoad()
    {
        Road road = CurrentRoad.Dequeue();

        _roadPool.Enqueue(road);
        road.gameObject.SetActive(false);
        _currentRoadCount--;
    }

    public void RestartLevel()
    {
        _lastRoad = null;

        while (_currentRoadCount > 0)
        {
            RemoveRoad();
        }

        CreateTrack();
    }

}
