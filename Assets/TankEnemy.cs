using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Enemy
{
    public Transform _shootingPoint;
    void Start()
    {
        base.SetUp();
    }

    private void FixedUpdate()
    {
        if(_player._died) return;
        _agent.SetDestination(_player.transform.position);
        Attacking();
        CheckStunned();
    }
    public void Attacking()
    {
        if(_stunned) return;

        var _distance = base.PlayerDistance();
        if(_distance > _attackRange) return;
        Debug.Log("Attacking !");
        if (_attackDelayCurrent > _attackDelayMax)
        {
            _attackDelayCurrent = 0;
            Attack(_shootingPoint);
            //shoot
        }
        else
        {
            _attackDelayCurrent += Time.deltaTime;
        }
    }
    
}
