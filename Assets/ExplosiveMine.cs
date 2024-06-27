using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMine : MonoBehaviour
{
    public float _damage;
    public int _damageRange;
    private GameManager _gameManager;
    public GameObject _explosionPrefab;
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        if(_gameManager._spawnedEnemies.Count == 0) return;
        if(Vector3.Distance(transform.position, GetNearestEnemy().position) < _damageRange)
            DoDamage();
    }

    private Transform GetNearestEnemy()
    {
        var lowestDist = Mathf.Infinity;
        GameObject _currentEnemy = null;
        foreach (var _enemy in _gameManager._spawnedEnemies)
        {
            if (_enemy == null) continue;
            //if(!RaycastEnemy(_enemy.transform)) continue;
            var dist = Vector3.Distance(_enemy.transform.position, transform.position);

            if (dist < lowestDist)
            {
                lowestDist = dist;
                _currentEnemy = _enemy;
            }
        }

        return _currentEnemy.transform;
    }
    private void DoDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _damageRange);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                var _enemy = col.GetComponent<Enemy>();
                Debug.Log(_enemy.name + " hit");
                if (_enemy.CheckHealth(_damage))
                {
                }
            }
        }
        GameObject _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(_explosion, 5);
        Destroy(gameObject);
    }
}
