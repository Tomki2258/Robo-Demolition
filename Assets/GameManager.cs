using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public List<Transform> _spawnPoints;
    private int _spawnsCount;
    public float _spawnTimeMax;
    private float _spawnTimeCurrent;
    private PlayerMovement _player;
    public float _spawnOffset;
    public List<GameObject> _enemies;
    public List<GameObject> _spawnedEnemies;
    private int _enemiesCount;
    private bool _gameLaunched = true;
    public float _powerUpSpawnTimeMax;
    private float _powerUpSpawnTimeCurrent;
    public List<Transform> _powerUpSpawnPoints;
    public GameObject _powerUps;
    private void Start()
    {
        _spawnsCount = _spawnPoints.Count;
        _player = FindAnyObjectByType<PlayerMovement>();
        foreach (Transform _obj in _spawnPoints)
        {
            _obj.name = "Enemy Spawn Point";
        }
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
    public void RemoveEnemy(GameObject _enemy)
    {
        _spawnedEnemies.Remove(_enemy);
    }
}
