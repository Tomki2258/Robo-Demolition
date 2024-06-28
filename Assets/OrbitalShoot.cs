using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalShoot : MonoBehaviour
{
    public float _damage;
    public float _damageRepeatMax;
    public float _damageRepeatCurrent;
    private Transform _body;
    private Renderer _renderer;
    private int _canonShots;
    public int _canonShotsMax;

    private void Start()
    {
        _body = transform.GetChild(0);
        _renderer = _body.GetComponent<Renderer>();
        _damageRepeatCurrent = _damageRepeatMax;
    }

    private void FixedUpdate()
    {
        if(_canonShots == _canonShotsMax) Destroy(gameObject);
        
        if(_damageRepeatCurrent < _damageRepeatMax)
        {
            _damageRepeatCurrent += Time.deltaTime;
        }
        else
        {
            _canonShots++;
            DoDamage();
        }
    }

    private void DoDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _renderer.bounds.size.x / 2);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                var _enemy = col.GetComponent<Enemy>();
                Debug.Log(_enemy.name + " hit");
                if (_enemy.CheckHealth(_damage))
                {
                }
            }
        } 
        _damageRepeatCurrent = 0;
    }
}
