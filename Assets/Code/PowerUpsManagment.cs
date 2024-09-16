using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpsManagment : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField] private List<Transform> _spawnersList;
    [SerializeField] private GameObject _powerUpPrefab;
    [SerializeField] private int _respawnMaxTime;
    private float _currentRestawnTime;
    [SerializeField] private List<GameObject> _spawnedPowerUps;
    [SerializeField] private int _maxSpawnedPowerUps;

    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
    }

    private void FixedUpdate()
    {
        if(!_gameManager._gameLaunched || !_gameManager._gameStarted) return;
        
        if (_currentRestawnTime < _respawnMaxTime)
        {
            _currentRestawnTime += Time.deltaTime;
            return;
        }

        _currentRestawnTime = 0;
        int _randomSpawner = Random.Range(0, _spawnersList.Count);
        Transform _selectedSpawner = _spawnersList[_randomSpawner];
        GameObject _currentPowerUp = Instantiate(_powerUpPrefab, _selectedSpawner.position, Quaternion.identity);
        _spawnedPowerUps.Add(_currentPowerUp);

        if (_spawnedPowerUps.Count == _maxSpawnedPowerUps + 1)
        {
            Destroy(_spawnedPowerUps[0]);
            _spawnedPowerUps.RemoveAt(0);
        }
    }
}
