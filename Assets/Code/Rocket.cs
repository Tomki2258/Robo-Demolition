using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Transform _enemy;
    private GameManager _gameManager;
    public float _rocketSpeed;
    private Vector3 _startTarget;
    private bool _starterDone;
    public GameObject _explosionFX;
    public int _rocketDamage;
    public float _rocketRange;
    void Start()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
        _startTarget = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
    }

    void FixedUpdate()
    {   
        if (!_starterDone)
        {
            transform.position = Vector3.MoveTowards(transform.position, _startTarget, _rocketSpeed * Time.deltaTime);
            RotateToTarget(_startTarget);
            if (GetDistance(_startTarget) < 0.1f)
                _starterDone = true;
            return;
        }
        _enemy = GetNearestEnemy();
        if (_enemy == null)
        {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, _enemy.position, _rocketSpeed * Time.deltaTime);
        _rocketSpeed *= 1.01f;
        RotateToTarget(_enemy.position);
        if(GetDistance(_enemy.position) < 0.1f)
            Destroy(gameObject);
    }

    private void RotateToTarget(Vector3 _target)
    {
        var _direction = _target - transform.position;
        _direction.Normalize();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), 10 * Time.deltaTime);
    }
    private float GetDistance(Vector3 _target)
    {
        return Vector3.Distance(transform.position, _target);
    }
    private Transform GetNearestEnemy()
    {
        Transform _currentEnemy = null;
        var lowestDist = Mathf.Infinity;
        foreach (var _enemy in _gameManager._spawnedEnemies)
        {
            var dist = Vector3.Distance(_enemy.transform.position, transform.position);

            if (dist < lowestDist)
            {
                lowestDist = dist;
                _currentEnemy = _enemy.transform;
            }
        }
        return _currentEnemy;
    }

    private void OnDestroy()
    {
        var _explosion = Instantiate(_explosionFX, transform.position, Quaternion.identity);
    }

    private void DoDamage()
    {
        Collider[] _colliders = Physics.OverlapSphere(transform.position, _rocketRange);
        foreach (Collider _obj in _colliders)
        {
            if (_obj.CompareTag("Enemy"))
            {
                _obj.GetComponent<Enemy>().CheckHealth(_rocketDamage);
            }
        }
    }
}
