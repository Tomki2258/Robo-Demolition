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
        if(_player._died) return;
        if(Vector3.Distance(_player.transform.position, transform.position) < _attackRange)
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
