using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterEnemy : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player._died) return;
        _agent.SetDestination(_player.transform.position);
        //Attacking();
        CheckStunned();
    }
}
