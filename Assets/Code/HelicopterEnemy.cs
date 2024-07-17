using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterEnemy : Enemy
{
    public Transform _shootingPoint;
    public Transform _waypoint;
    void Start()
    {
        _agent.SetDestination(_player.transform.position);
        SetUp();
    }

    void FixedUpdate()
    {
        DoWings();
        if (_player._died) return;
        SetPlayerTarget();
        CheckStunned();
        SwitchSpeed();
        if(Vector3.Distance(_player.transform.position, transform.position) < _attackRange)
        {
            Attacking();
        }
    }

    private void Attacking()
    {
        if(_attackDelayCurrent > _attackDelayMax)
        {
            _attackDelayCurrent = 0;
            Attack(_shootingPoint);
        }
        else
        {
            _attackDelayCurrent += Time.deltaTime;
        }
    }
}
