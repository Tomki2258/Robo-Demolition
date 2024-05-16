using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketEnemy : Enemy
{
    [Header("Rocket enemy elements")]
    public GameObject _rocketPrefab;

    public GameObject _rocketTargetPrefab;
    public Transform _shootTransform;
    private void Start()
    {
        base.SetUp();
    }

    private void FixedUpdate()
    {
        if (_player._died) return;
        
        _agent.SetDestination(_player.transform.position);

        if (Vector3.Distance(transform.position, _player.transform.position) < _attackRange)
        {
            if (_attackDelayCurrent > _attackDelayMax)
            {
                GameObject _currentRocketTarget = Instantiate(_rocketTargetPrefab,
                    _player.transform.position,
                    Quaternion.identity);
            
            
                _attackDelayCurrent = 0;

                GameObject _currentRocket = Instantiate(_rocketPrefab, 
                    _shootTransform.position, 
                    Quaternion.identity);
                Rocket _currentRocketComponent = _currentRocket.GetComponent<Rocket>();
                _currentRocketComponent._isEnemy = true;
                _currentRocketComponent._enemy = _currentRocketTarget.transform;
                //shoot
            }
            else
            {
                _attackDelayCurrent += Time.deltaTime;
            }   
        }
        CheckStunned();
    }
}
