using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    [SerializeField] private GameObject[] _prefabs;
    private List<GameObject>[] _pools;

    public GameObject[] Prefabs => _prefabs;
    public List<GameObject>[] Pools => _pools;

    void Awake() {
        _pools = new List<GameObject>[_prefabs.Length];

        for (int i = 0; i < _pools.Length; i++) {
            _pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index) {
        GameObject select = null;

        foreach (GameObject item in _pools[index]) {
            if (!item.activeSelf) {
                select = item;
                select.SetActive(true);
                
                break;
            }
        }

        if (!select) {
            select = Instantiate(_prefabs[index], transform);
            _pools[index].Add(select);
        }
            
        return select;
    }
}

