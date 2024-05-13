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
    public bool _isEnemy;
    private PlayerMovement _player;
    private Vector3 _lastKnownPosition;
    void Start()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
        _startTarget = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
        _player = FindFirstObjectByType<PlayerMovement>();
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

        if (!_isEnemy)
            _enemy = GetNearestEnemy();
        /*
        else
            _enemy = _player.transform;
            */

        if (_enemy != null)
        {
            _lastKnownPosition = _enemy.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, _lastKnownPosition,
            _rocketSpeed * Time.deltaTime);
        _rocketSpeed *= 1.01f;
        RotateToTarget(_lastKnownPosition);
        if(GetDistance(_lastKnownPosition) < 0.1f)
        {
            var _explosion = Instantiate(_explosionFX, transform.position, Quaternion.identity);
            DoDamage();
            Destroy(gameObject);
        }
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
            if(_enemy == null) continue;
            var dist = Vector3.Distance(_enemy.transform.position, transform.position);

            if (dist < lowestDist)
            {
                lowestDist = dist;
                _currentEnemy = _enemy.transform;
            }
        }
        return _currentEnemy;
    }
    private void DoDamage()
    {
        Collider[] _colliders = Physics.OverlapSphere(transform.position, _rocketRange);
        foreach (Collider _obj in _colliders)
        {
            if (_enemy)
            {   
                Destroy(_enemy.gameObject);
                if (_obj.CompareTag("Player"))
                {
                    _player.CheckHealth(_rocketDamage);
                    return;
                }
            }
            if (_obj.CompareTag("Enemy"))
            {
                _obj.GetComponent<Enemy>().CheckHealth(_rocketDamage);
            }
        }
    }
}
