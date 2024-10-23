using System.Collections.Generic;
using UnityEngine;

public class ConstructorEnemy : Enemy
{
    [Header("Constructor Enemy")] [SerializeField]
    private GameObject _turretPrefab;
    private List<GameObject> _placedTurrets;
    private int _maxTurrets = 3;
    void Start()
    {
        SetUp();
    }

    void FixedUpdate()
    {
        LookForColorChange();
        //SwitchLight();
        var _distance = PlayerDistance();
        SetPlayerTarget();
        SwitchSpeed();
        //if(_distance < _attackRange) 
        CheckStunned();
    }
}
