using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyFactory : MonoBehaviour
{
    [Header("Enemies Section")]
    public List<GameObject> _enemies;
    public List<int> _enemiesStages;
    public int _possibleEnemies;
    public List<GameObject> _spawnedEnemies;
    private int _spawnsCount;
    public float _spawnOffset;
    public List<Transform> _spawnPoints;
    public PlayerMovement _player;

    private void Start()
    {
        //_player = FindFirstObjectByType<PlayerMovement>();
    }

    public float _spawnTimeMax;

    public void SpawnEnemy()
    {
        var _point = Random.Range(0, _spawnsCount);
        var _distance = Vector3.Distance(_spawnPoints[_point].position, _player.transform.position);
        if (_distance < _spawnOffset) 
            _point = Random.Range(0, _spawnsCount);

        // Spawn enemy
        var _enemyIndex = Random.Range(0, _possibleEnemies);
        var _randomSpawnVector = new Vector3(_spawnPoints[_point].position.x + Random.Range(-10, 10),
            _spawnPoints[_point].position.y,
            _spawnPoints[_point].position.z + Random.Range(-10, 10));
        var _enemy = Instantiate(_enemies[_enemyIndex], _randomSpawnVector, Quaternion.identity);
        _spawnTimeMax -= _spawnTimeMax * 0.002f;

        if (_enemy.GetComponent<BombardEnemy>()) return;

        _spawnedEnemies.Add(_enemy);
        //_enemy.GetComponent<Enemy>()._gameManager = this;
        _enemy.GetComponent<Enemy>()._player = _player;
    }
}
