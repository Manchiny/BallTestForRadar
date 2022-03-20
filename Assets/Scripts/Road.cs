using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private List<Transform> _itemSpawnPoints;
    [SerializeField] private GameObject _moneyPrefab;
    [SerializeField] private GameObject _bombPrefab;
    public EndRoadChecker EndRoadChecker { get; private set; }

    private List<GameObject> _allItems;
    private void Awake()
    {
        EndRoadChecker = GetComponentInChildren<EndRoadChecker>();

        _allItems = new List<GameObject>();
    }

    public void CreateRandomItems()
    {
        foreach (var point in _itemSpawnPoints)
        {
            int random = Random.Range(0, 3);
            GameObject prefab = null;
            if (random == 1)
                prefab = _moneyPrefab;
            else if (random == 2)
                prefab = _bombPrefab;

            if(prefab != null)
            {
                float pointY = point.position.y;
                Vector3 randomPoint = Random.insideUnitSphere + point.position;
                randomPoint.y = pointY;
                var item = Instantiate(prefab, randomPoint, Quaternion.identity);
                item.transform.SetParent(transform);
                _allItems.Add(item);
            }
        }
    }

    public void ShowItems()
    {
        if(_allItems !=null)
        {
            foreach (var item in _allItems)
            {
                item.SetActive(true);
            }
        }
    }
}
