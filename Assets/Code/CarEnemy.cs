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
        //SwitchLight();

        var _distance = PlayerDistance();
        SetPlayerTarget();
        if(_distance < _attackRange) Attacking();
        CheckStunned();
    }

    /*
    private void SwitchLight()
    {
        if (_lightsTimerMax > _lightsTimer)
        {
            _lightsTimer += Time.deltaTime;
        }
        else
        {
            _lightsTimer = 0;
            _lights[0].SetActive(_lights[0].activeSelf);
            _lights[1].SetActive(!_lights[1].activeSelf);
        }
    }
    */
    public void Attacking()
    {
        if (_stunned || _player._died) return;

        if (_attackDelayCurrent > _attackDelayMax)
        {
            _attackDelayCurrent = 0;
            if (_shootingPointIndex == 0) _shootingPointIndex = 1;
            else _shootingPointIndex = 0;
            Attack(_shootingPoints[_shootingPointIndex]);
            _shootingPoints[0].LookAt(_player.transform.position);
            _shootingPoints[1].LookAt(_player.transform.position);
            //shoot
        }
        else
        {
            _attackDelayCurrent += Time.deltaTime;
        }
    }
}