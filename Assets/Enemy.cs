using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent _agent;
    public GameManager _gameManager;
    public PlayerMovement _player;
    public GameObject _explosionPrefab;
    public int _attackRange;
    public int _incomingRange;
    public int _xpReward;
    public GameObject _bullet;
    public int _bulletDamage;
    private CameraShake _cameraShake;
    private float _lightsTimer;
    private readonly float _lightsTimerMax = 1;
    private int health = 10;
    public float _attackDelayMax;
    public float _attackDelayCurrent;
    public void SetUp()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (_player == null) Destroy(gameObject);
    }
    public void CheckHealth(int _value)
    {
        health -= _value;
        if (health <= 0)
            Destroy(gameObject);
    }

    public float PlayerDistance()
    {
        return Vector3.Distance(transform.position, _player.transform.position);
    }
    private void OnDestroy()
    {
        _gameManager.RemoveEnemy(gameObject);
        //GameObject _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        //_cameraShake.DoShake(0.001f, 1);
        _player._xp += _xpReward;
        //Destroy(_explosion,5);
    }
    
    public void Attack(Transform _bulletSpawn)
    {
        _bulletSpawn.LookAt(_player.transform.position);
        var _bulletInstance = Instantiate(_bullet, _bulletSpawn.position,
            _bulletSpawn.rotation);
        var _bulletScript = _bulletInstance.GetComponent<Bullet>();
        _bulletScript._enemyShoot = true;
        _bulletScript._bulletDamage = _bulletDamage;
        Destroy(_bulletInstance, 5);
    }
}