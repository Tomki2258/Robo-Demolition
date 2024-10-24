using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buldoger : Enemy
{
    void Start()
    {
        SetUp();
    }

    void FixedUpdate()
    {
        SetPlayerTarget();
        PlayerDistance();
        if(_player._died) return;
        if(_playerDistance < _attackRange)
        {
            Attacking();
        }
        CheckStunned();
    }

    private void Attacking()
    {
        if(_attackDelayCurrent > _attackDelayMax)
        {
            _attackDelayCurrent = 0;
            _player.CheckHealth(_bulletDamage);
        }
        else
        {
            _attackDelayCurrent += Time.deltaTime;
        }
    }
}
