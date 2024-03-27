using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Transform _enemy;
    private GameManager _gameManager;
    public float _rocketSpeed;
    void Start()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        _enemy = GetNearestEnemy();
        transform.position = Vector3.MoveTowards(transform.position, _enemy.position, _rocketSpeed * Time.deltaTime);
        var _direction = _enemy.transform.position - transform.position;
        _direction.Normalize();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), 10 * Time.deltaTime);
        
        if(GetDistance(_enemy) < 0.1f)
            Destroy(gameObject);
    }

    private float GetDistance(Transform _target)
    {
        return Vector3.Distance(transform.position, _target.position);
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
}
