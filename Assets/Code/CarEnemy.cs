using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Incoming,
    Attacking
}

public class CarEnemy : Enemy
{
    public State _currentState;
    public List<Transform> _shootingPoints;
    public List<GameObject> _lights;
    private int _shootingPointIndex;

    private void Awake()
    {
        SetUp();
    }

    private void FixedUpdate()
    {
        LookForColorChange();
        //SwitchLight();
        PlayerDistance();
        SetPlayerTarget();
        SwitchSpeed();
        if(_playerDistance < _attackRange) Attacking();
        CheckStunned();
    }
    private void Attacking()
    {
        if (_stunned || _player._died) return;

        if (_attackDelayCurrent > _attackDelayMax)
        {
            _attackDelayCurrent = 0;
            Attack(_shootingPoints[0],0);
            _shootingPoints[0].LookAt(_player.transform.position);
            //shoot
        }
        else
        {
            _attackDelayCurrent += Time.deltaTime;
        }
    }
}