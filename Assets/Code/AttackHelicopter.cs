using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHelicopter : Enemy
{
    public GameObject _rocketPrefab;
    public Transform _shootingPoint;
    public float _rocketDelayCurrent;
    public float _rocketDelayMax;
    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _attackRange = Convert.ToInt32(_player.GetRealAttackRange() + 10);
        _stoppingDistance = Convert.ToInt32(_player.GetRealAttackRange() + 5);
        DoWings();
        SetPlayerTarget();

        if (_player._died) return;
        SetPlayerTarget();
        CheckStunned();
        if(Vector3.Distance(_player.transform.position, transform.position) < _attackRange)
        {
            Attacking();
        }
    }

    private void Attacking()
    {
        if (_attackDelayCurrent > _attackDelayMax)
        {
            _attackDelayCurrent = 0;
            Attack(_shootingPoint);
        }
        else
        {
            _attackDelayCurrent += Time.deltaTime;
        }
        
        if(_rocketDelayCurrent > _rocketDelayMax)
        {
            RocketAttack();
            _rocketDelayCurrent = 0;
        }
        else
        {
            _rocketDelayCurrent += Time.deltaTime;
        }
    }

    private void RocketAttack()
    {
        var _currentRocket = Instantiate(_rocketPrefab,
            transform.position,
            Quaternion.identity);
        Rocket _currentRocketComponent = _currentRocket.GetComponent<Rocket>();
        _currentRocketComponent._starterDone = true;
        _currentRocketComponent._isEnemy = true;
        _currentRocketComponent._enemy = _player.transform;
        _audioSource.PlayOneShot(_shootAudioClip);
        _rocketDelayCurrent = 0;
    }
}
