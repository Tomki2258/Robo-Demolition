using System.Collections.Generic;
using UnityEngine;

public enum ConstructorState
{
    Incoming,
    Building,
}

public class ConstructorEnemy : Enemy
{
    [Header("Constructor Enemy")] [SerializeField]
    private GameObject _turretPrefab;

    private List<GameObject> _placedTurrets = new List<GameObject>();
    private int _maxTurrets = 3;
    public ConstructorState _constructorState;
    private float _buildTimer;
    [SerializeField] float _buildTimerMax;
    private float _lastAgentSpeed;
    private Animator _animator;
    void Start()
    {
        SetUp();
        _lastAgentSpeed = _agent.speed;
        _constructorState = ConstructorState.Incoming;

        _attackDelayCurrent = 0;
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        LookForColorChange();
        PlayerDistance();
        switch (_constructorState)
        {
            case ConstructorState.Incoming:
                SetPlayerTarget();
                SwitchSpeed();
                CheckStunned();

                Incoming();

                break;
            case ConstructorState.Building:
                BuildTurret();
                break;
        }
    }

    private void Incoming()
    {
        if (_attackDelayCurrent > _attackDelayMax)
        {
            _constructorState = ConstructorState.Building;
            _attackDelayCurrent = 0;
        }
        else
        {
            _attackDelayCurrent += Time.deltaTime;
        }
    }

    private void BuildTurret()
    {
        if (_buildTimer < _buildTimerMax)
        {
            _buildTimer += Time.deltaTime;
            _agent.speed = 0;
            return;
        }

        _constructorState = ConstructorState.Incoming;
        _agent.speed = _lastAgentSpeed;
        _buildTimer = 0;
        GameObject _curentTurret = Instantiate(_turretPrefab, transform.position, Quaternion.identity);
        _placedTurrets.Add(_curentTurret);

        if (_placedTurrets.Count > _maxTurrets)
        {
            GameObject _firstTurret = _placedTurrets[0];
            _placedTurrets.Remove(_firstTurret);
            _firstTurret.GetComponent<Turret>().DestroyTurret();
        }
    }
}