using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [Header("# Player Object")]
    [SerializeField] private PlayerController _player;
    [SerializeField] private PoolManager _pm;
    private static GameManager _instance;
    [SerializeField] private LevelUp _uiLevelUp;
    [SerializeField] private Result _uiResult;
    [SerializeField] private GameObject _enemyCleaner;
    
    
    [Header("# Game Control")]
    public float _currentTime;
    public float _maxGameTime;
    private bool _isLive;

    [Header("# Player Info")]
    public int _playerId;
    public float _hp;
    public float _maxHp;
    public int _level;
    public int _kill;
    public int _exp;
    public int[] _nextEXP = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };
    
    public static GameManager Instance { get => _instance; }

    public PlayerController Player => _player;
    public PoolManager PM => _pm;

    public bool IsLive { get => _isLive; }

    private void Awake() {
        _instance = this;
    }

    public void GameStart(int id) {
        _playerId = id;
        _hp = _maxHp;
        
        _player.gameObject.SetActive(true);
        _uiLevelUp.Select(_playerId % 2);
        Resume();
        
        AudioManager.Instance.PlayBgm(true);
        AudioManager.Instance.PlaySfx(AudioManager.SFX.Select);
    }

    public void GameOver() {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine() {
        _isLive = false;
        
        yield return new WaitForSeconds(0.5f);
        
        _uiResult.gameObject.SetActive(true);
        _uiResult.Lose();
        Stop();
        
        AudioManager.Instance.PlayBgm(false);
        AudioManager.Instance.PlaySfx(AudioManager.SFX.Lose);
    }
    
    public void GameVictory() {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine() {
        _isLive = false;
        _enemyCleaner.SetActive(true);
        
        yield return new WaitForSeconds(0.5f);
        
        _uiResult.gameObject.SetActive(true);
        _uiResult.Victory();
        Stop();
        
        AudioManager.Instance.PlayBgm(false);
        AudioManager.Instance.PlaySfx(AudioManager.SFX.Win);
    }
    
    public void GameRetry() {
        SceneManager.LoadScene(0);
    }

    private void Update() {
        if (!_isLive) return;
        
        _currentTime += Time.deltaTime;
        
        if (_currentTime > _maxGameTime) {
            _currentTime = _maxGameTime;
            GameVictory();
        }
    }

    public void GetExp() {
        if (!_isLive) return;
        
        _exp++;
        if (_exp == _nextEXP[Mathf.Min(_level, _nextEXP.Length-1)]) {
            _exp -= _nextEXP[Mathf.Min(_level, _nextEXP.Length-1)];
            _level++;
            _uiLevelUp.Show();
        }
    }

    public void Stop() {
        _isLive = false;
        Time.timeScale = 0;
    }
    
    public void Resume() {
        _isLive = true;
        Time.timeScale = 1;
    }
}

