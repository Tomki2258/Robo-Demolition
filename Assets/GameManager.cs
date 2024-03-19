using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public List<Transform> _spawnPoints;
    private int _spawnsCount;
    public float _spawnTimeMax;
    private float _spawnTimeCurrent;
    public PlayerMovement _player;
    public float _spawnOffset;
    public List<GameObject> _enemies;
    public List<GameObject> _spawnedEnemies;
    private int _enemiesCount;
    private bool _gameLaunched = true;
    public float _powerUpSpawnTimeMax;
    private float _powerUpSpawnTimeCurrent;
    public List<Transform> _powerUpSpawnPoints;
    private List<GameObject> _spawnedPowerUps; 
    public GameObject _powerUps;
    private void Awake()
    {
        _spawnsCount = _spawnPoints.Count;
        _player = FindAnyObjectByType<PlayerMovement>();
        foreach (Transform _obj in _spawnPoints)
        {
            _obj.name = "Enemy Spawn Point";
        }
        SpawnPowerUp();
    }

    void Update()
    {
        if(!_gameLaunched) return;

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
        int _point = Random.Range(0, _spawnsCount);
        float _distance = Vector3.Distance(_spawnPoints[_point].position, _player.transform.position);
        if(_distance < _spawnOffset) return;
        
        // Spawn enemy
        int _enemyIndex = Random.Range(0, _enemiesCount);
        GameObject _enemy = Instantiate(_enemies[_enemyIndex], _spawnPoints[_point].position, Quaternion.identity);
        _spawnedEnemies.Add(_enemy);
        _enemy.GetComponent<Enemy>()._gameManager = this;
        _enemy.GetComponent<Enemy>()._player = _player;
        _spawnTimeMax -= _spawnTimeMax*0.001f;
    }
    private void SpawnPowerUp()
    {
        int _point = Random.Range(0, _spawnsCount);
        float _distance = Vector3.Distance(_spawnPoints[_point].position, _player.transform.position);
        if(_distance < _spawnOffset) return;
        
        // Spawn enemy
        int _powerUpIndex = Random.Range(0, _enemiesCount);
        GameObject _currentPowerUp = Instantiate(_enemies[_powerUpIndex], _spawnPoints[_point].position, Quaternion.identity);
        _spawnedPowerUps.Add(_currentPowerUp);
    }
    public void RemoveEnemy(GameObject _enemy)
    {
        _spawnedEnemies.Remove(_enemy);
    }
}
