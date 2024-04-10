using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public bool _gameLaunched;

    public List<Transform> _spawnPoints;
    public float _spawnTimeMax;
    public PlayerMovement _player;
    public float _spawnOffset;
    public List<GameObject> _enemies;
    public List<GameObject> _spawnedEnemies;
    public float _powerUpSpawnTimeMax;
    public List<Transform> _powerUpSpawnPoints;
    public GameObject _powerUps;
    public GameObject _pausedUI;
    private int _enemiesCount;

    private bool _paused;
    private float _powerUpSpawnTimeCurrent;
    private List<GameObject> _spawnedPowerUps;
    private int _spawnsCount;
    public float _spawnTimeCurrent;
    public int _killedEnemies;
    private InterstitialAd _interstitialAd;
    public GameSettings _gameSettings;
    private UIManager _uiManager;
    public GameObject _explosion;
    private void Awake()
    {
        DestroyTrash();
        
        _spawnsCount = _spawnPoints.Count;
        _enemiesCount = _enemies.Count;
        _player = FindAnyObjectByType<PlayerMovement>();
        foreach (var _obj in _spawnPoints) _obj.name = "Enemy Spawn Point";
        _pausedUI.SetActive(false);
        if(!_gameLaunched) _player.DoJoystickInput(false);
        _interstitialAd = FindFirstObjectByType<InterstitialAd>();
        _gameSettings = FindFirstObjectByType<GameSettings>();
        _uiManager = FindFirstObjectByType<UIManager>();
    }

    private void DestroyTrash()
    {
        GameObject[] _trashArray;
        _trashArray = GameObject.FindGameObjectsWithTag("Trash");
        foreach (GameObject _trash in _trashArray) 
        {
            Destroy(_trash);
        }
    }
    private void FixedUpdate()
    {
        if (!_gameLaunched || _player._died)
        {
            Debug.Log("Player died");
            return;
        }
        //if(!_player._died) return;

        if (_spawnTimeCurrent < _spawnTimeMax)
        {
            _spawnTimeCurrent += Time.deltaTime;
            return;
        }

        SpawnEnemy();
        _spawnTimeCurrent = 0;
    }
    
    private void SpawnEnemy()
    {
        var _point = Random.Range(0, _spawnsCount);
        var _distance = Vector3.Distance(_spawnPoints[_point].position, _player.transform.position);
        if (_distance < _spawnOffset) return;

        // Spawn enemy
        var _enemyIndex = Random.Range(0, _enemiesCount);
        var _enemy = Instantiate(_enemies[_enemyIndex], _spawnPoints[0].position, Quaternion.identity);
        _spawnedEnemies.Add(_enemy);
        _enemy.GetComponent<Enemy>()._gameManager = this;
        _enemy.GetComponent<Enemy>()._player = _player;
        _spawnTimeMax -= _spawnTimeMax * 0.001f;
    }

    private void SpawnPowerUp()
    {
        var _point = Random.Range(0, _spawnsCount);
        var _distance = Vector3.Distance(_powerUpSpawnPoints[_point].position, _player.transform.position);
        if (_distance < _spawnOffset) return;

        //var _powerUpIndex = Random.Range(0, _enemiesCount);
        var _currentPowerUp = Instantiate(_powerUps, _powerUpSpawnPoints[_point].position, Quaternion.identity);
        _spawnedPowerUps.Add(_currentPowerUp);
    }

    public void RemoveEnemy(GameObject _enemy)
    {
        _spawnedEnemies.Remove(_enemy);
    }

    public void PauseGame()
    {
        _paused = !_paused;
        if (_paused)
        {
            Time.timeScale = 0;
            _pausedUI.SetActive(true);
            _uiManager._mainUI.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            _pausedUI.SetActive(false);
            _uiManager._mainUI.SetActive(true);
        }
    }

    public void DoAd()
    {
        int _random = Random.Range(0, 3);
        if (_random == 0)
        {
            _interstitialAd.ShowAd();
        }
    }
    public void AdReward()
    {
        Debug.LogWarning("Ad reward");
        foreach (GameObject _enemy in _spawnedEnemies)
        {
            _enemy.GetComponent<Enemy>()._killedByManager = true;
            Destroy(_enemy);
        }
        _spawnedEnemies.Clear();

        _uiManager._dieCanvas.SetActive(false);
        _uiManager._mainUI.SetActive(true);
        
        _player.Revive();
    }

    public IEnumerator ReloadLevel()
    {
        _uiManager._dieCanvas.SetActive(false);
        GameObject _explosionn = Instantiate(_explosion,_player.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.loadedSceneCount);
    }
}