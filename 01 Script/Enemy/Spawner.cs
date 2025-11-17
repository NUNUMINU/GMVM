using UnityEngine;

public class Spawner : MonoBehaviour {
    
    private int _level;
    private float _timer;

    private PoolManager _pm;
    private Transform[] _spawnPoint;
    [SerializeField] private SpawnData[] _spawnDatas;
    private float _levelTime;
    
    void Awake() {
        _spawnPoint = GetComponentsInChildren<Transform>();
        _levelTime = GameManager.Instance._maxGameTime / _spawnDatas.Length;
    }
    void Start() {
        _pm = GameManager.Instance.PM;
    }

    void Update() {
        if (!GameManager.Instance.IsLive) return;
        
        _level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance._currentTime / _levelTime), _spawnDatas.Length - 1);
        _timer += Time.deltaTime;
        
        if (_timer > _spawnDatas[_level]._spawnTime) {
            Spawn();

            _timer -= _spawnDatas[_level]._spawnTime;
        }
    }

    void Spawn() {
        GameObject enemy = _pm.Get(0);
        enemy.transform.position = _spawnPoint[Random.Range(1, _spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(_spawnDatas[_level]);
    }
}

[System.Serializable]
public class SpawnData {
    public int _spriteType;
    public int _hp;
    public float _movSpeed;
    public float _spawnTime;
}
