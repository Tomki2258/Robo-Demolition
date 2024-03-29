using System;
using System.Collections.Generic;
using UnityEngine;
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
    public bool _qualityOn;
    public Sprite _qualityOnSprite;
    public Sprite _qualityOffSprite;
    public Image _qualityImage;
    private int _enemiesCount;

    private bool _paused;
    private float _powerUpSpawnTimeCurrent;
    private List<GameObject> _spawnedPowerUps;
    private int _spawnsCount;
    public float _spawnTimeCurrent;

    private void Awake()
    {
        _spawnsCount = _spawnPoints.Count;
        _enemiesCount = _enemies.Count;
        _player = FindAnyObjectByType<PlayerMovement>();
        foreach (var _obj in _spawnPoints) _obj.name = "Enemy Spawn Point";
        _pausedUI.SetActive(false);
        LoadQuality();
    }

    private void FixedUpdate()
    {
        if (!_gameLaunched) return;
        //if(!_player._died) return;

        if (_spawnTimeCurrent < _spawnTimeMax)
        {
            _spawnTimeCurrent += Time.deltaTime;
            return;
        }

        SpawnEnemy();
        _spawnTimeCurrent = 0;
    }

    private void LoadQuality()
    {
        QualitySettings.vSyncCount = 1;

        var _savedQuality = Convert.ToBoolean(PlayerPrefs.GetInt("SavedQuality"));
        if (_savedQuality)
        {
            QualitySettings.SetQualityLevel(4);
            _qualityImage.sprite = _qualityOnSprite;
        }
        else
        {
            QualitySettings.SetQualityLevel(0);
            _qualityImage.sprite = _qualityOffSprite;
        }
    }

    public void SwitchQualitySettings()
    {
        _qualityOn = !_qualityOn;
        if (_qualityOn)
        {
            QualitySettings.SetQualityLevel(4);
            _qualityImage.sprite = _qualityOnSprite;
        }
        else
        {
            QualitySettings.SetQualityLevel(0);
            _qualityImage.sprite = _qualityOffSprite;
        }

        PlayerPrefs.SetInt("SavedQuality", _qualityOn ? 1 : 0);
    }

    private void SpawnEnemy()
    {
        var _point = Random.Range(0, _spawnsCount);
        var _distance = Vector3.Distance(_spawnPoints[_point].position, _player.transform.position);
        if (_distance < _spawnOffset) return;

        // Spawn enemy
        var _enemyIndex = Random.Range(0, _enemiesCount);
        var _enemy = Instantiate(_enemies[_enemyIndex], _spawnPoints[_point].position, Quaternion.identity);
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
        }
        else
        {
            Time.timeScale = 1;
            _pausedUI.SetActive(false);
        }
    }
}