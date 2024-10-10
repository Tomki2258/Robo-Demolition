using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private GameManager _gameManager;
    private PlayerMovement _player;
    [SerializeField] private int _attackRange;
    private float _attackTimer;
    private int _attackTimerMax;
    [SerializeField] private Transform _rotateTop;
    private float _rotateSpeed = 5f;
    [SerializeField] private int _damage;
    private float _health;
    private void Start()
    {
        _player = FindObjectOfType<PlayerMovement>();
        _gameManager = FindObjectOfType<GameManager>();
        _gameManager._spawnedEnemies.Add(gameObject);
    }

    private void FixedUpdate()
    {
        MoveTurret(_player.transform.position);

        if (Vector3.Distance(transform.position, _player.transform.position) < _attackRange)
        {
            Attack();
        }
        //tamusUtils.MoveTurret(_player.transform.position, transform, _rotateTop,_rotateSpeed);
    }
    private void Attack()
    {
        if(_attackTimer < _attackTimerMax)
        {
            _attackTimer += Time.deltaTime;
            return;
        }
        
        _attackTimer = 0;
        
        // Attack logic
    }
    private void MoveTurret(Vector3 _target)
    {
        var _direction = _target - transform.position;
        _direction.Normalize();
        _rotateTop.rotation = Quaternion.Slerp(_rotateTop.rotation, Quaternion.LookRotation(_direction),
            _rotateSpeed * Time.deltaTime);
    }
    public float ReceiveDamage(int _damage)
    {
        _health -= _damage;
        if (_health <= 0)
        {
            DestroyTurret();
            return 0;
        }
        return _health;
    }

    private void DestroyTurret()
    {
        Destroy(gameObject);
    }
}

