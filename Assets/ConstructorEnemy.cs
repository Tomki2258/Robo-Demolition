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
    private List<GameObject> _placedTurrets;
    private int _maxTurrets = 3;
    public ConstructorState _constructorState;
    private float _buildTimer;
    private float _buildTimerMax = 10;
    void Start()
    {
        SetUp();
        
        _constructorState = ConstructorState.Incoming;
    }

    private void FixedUpdate()
    {
        LookForColorChange();
        var _distance = PlayerDistance();
        switch (_constructorState)
        {
            case ConstructorState.Incoming:
                if (_distance < _attackRange)
                {
                    SetPlayerTarget();
                    SwitchSpeed();
                    CheckStunned();
                    
                    Incoming();
                }
                break;
            case ConstructorState.Building:
                Building();
                break;
        }
    }

    private void Incoming()
    {
        if(_attackDelayCurrent > _attackDelayMax)
        {
            _attackDelayCurrent = 0;
            _constructorState = ConstructorState.Building;
        }
        else
        {
            _attackDelayCurrent += Time.deltaTime;
        }
    }
    private void Building()
    {
        if(_buildTimer < _buildTimerMax)
        {
            _buildTimer += Time.deltaTime;
            return;
        }
        
        GameObject _curentTurret = Instantiate(_turretPrefab, transform.position, Quaternion.identity);
        _placedTurrets.Add(_curentTurret);
        
        if(_placedTurrets.Count >= _maxTurrets)
        {
            Turret _turret = _curentTurret.GetComponent<Turret>();
            _turret.DestroyTurret();
        }
        
        _constructorState = ConstructorState.Incoming;
    }
}
