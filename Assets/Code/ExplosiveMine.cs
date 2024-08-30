using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMine : MonoBehaviour
{
    public float _damage;
    public int _damageRange;
    private GameManager _gameManager;
    public GameObject _explosionPrefab;
    private bool _ready;
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(Prepare());
    }

    IEnumerator Prepare()
    {
        yield return new WaitForSeconds(3);
        _ready = true;
    }
    private void FixedUpdate()
    {
        if(_gameManager._spawnedEnemies.Count == 0 || !_ready) return;
        if(EnemyInRange())
            DoDamage();
    }

    private bool EnemyInRange()
    {
        Collider[] _colliders = Physics.OverlapSphere(transform.position, _damageRange);
        foreach (Collider _col in _colliders)
        {
            if (_col.GetComponent<Enemy>())
                return true;
        }

        return false;
    }
    private void DoDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _damageRange);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                var _enemy = col.GetComponent<Enemy>();
                if (_enemy.CheckHealth(_damage))
                {
                }
            }
        }
        GameObject _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(_explosion, 5);
        Destroy(gameObject);
    }
}
