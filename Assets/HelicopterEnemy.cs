using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterEnemy : Enemy
{
    void Start()
    {
        SetUp();
    }

    void FixedUpdate()
    {
        if (_player._died) return;
        _agent.SetDestination(_player.transform.position);
        CheckStunned();
        DoWings();
    }
}
