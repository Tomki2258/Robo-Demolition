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

    //private List<GameObject> _placedTurrets = new List<GameObject>();
    private int _maxTurrets = 3;
    public ConstructorState _constructorState;
    private float _buildTimer;
    [SerializeField] float _buildTimerMax;
    private float _lastAgentSpeed;
    private Animator _animator;
    private float _switchBuildPositionTimer;
    private float _switchBuildPositionTimerMax = 0.5f;
    private Transform _child;
    [SerializeField] private List<Transform> _handsTransforms;
    private Vector3 _startHandRotation;
    void Start()
    {
        SetUp();
        _lastAgentSpeed = _agent.speed;
        _constructorState = ConstructorState.Incoming;

        _attackDelayCurrent = 0;
        _animator = GetComponent<Animator>();
        _startHandRotation = _handsTransforms[0].localEulerAngles;
        _child = transform.GetChild(0);
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

    private void RotateHands()
    {
        for (int i = 0; i < _handsTransforms.Count; i++)
        {
            _handsTransforms[i].Rotate(10,0,0,Space.Self);
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
            SetRandomBuildPosition();
            RotateHands();
            _buildTimer += Time.deltaTime;
            _agent.speed = 0;
            return;
        }

        _constructorState = ConstructorState.Incoming;
        _agent.speed = _lastAgentSpeed;
        _buildTimer = 0;
        GameObject _curentTurret = Instantiate(_turretPrefab, transform.position, Quaternion.identity);
        //_placedTurrets.Add(_curentTurret);
        _enemiesManager.AddTurrte(_curentTurret);
        _child.transform.position = transform.position;
        _child.LookAt(_player.transform.position);
        _switchBuildPositionTimer = 0;
        
        _enemiesManager.CheckTurrets();
        /*
        if (_placedTurrets.Count > _maxTurrets)
        {
            GameObject _firstTurret = _placedTurrets[0];
            _placedTurrets.Remove(_firstTurret);
            _firstTurret.GetComponent<Turret>().DestroyTurret();
        }
        */
    }

    private void SetRandomBuildPosition()
    {
        if(_switchBuildPositionTimer < _switchBuildPositionTimerMax)
        {
            _switchBuildPositionTimer += Time.deltaTime;
            return;
        }
        
        Vector3 _randomPosition = new Vector3(transform.position.x + Random.Range(-2, 2),
            transform.position.y  + 0,
            transform.position.z + Random.Range(-2, 2));
        _child.position = _randomPosition;
        _child.LookAt(transform.position);
        
        _switchBuildPositionTimer = 0;
    }
}