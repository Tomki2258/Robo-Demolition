using System;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private GameManager _gameManager;
    private PlayerMovement _player;
    public EnemiesManager _enemiesManager;
    [SerializeField] private int _attackRange;
    private float _attackTimer;
    public float _attackTimerMax;
    [SerializeField] private Transform _rotateTop;
    [SerializeField] private Transform _shootTransform;
    public  Vector3 _offset;
    private float _rotateSpeed = 5f;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private int _damage;
    public float _health;
    public float _maxHealth;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _shootAudioClip;
    private void Start()
    {
        _player = FindObjectOfType<PlayerMovement>();
        _gameManager = FindObjectOfType<GameManager>();
        _gameManager._spawnedEnemies.Add(gameObject);
        _maxHealth = _health;
    }

    private void FixedUpdate()
    {
        MoveTurret(_player.transform.position);

        if (Vector3.Distance(transform.position, _player.transform.position) < _attackRange)
        {
            Attack();
        }
    }
    private void Attack()
    {
        if(_attackTimer < _attackTimerMax)
        {
            _attackTimer += Time.deltaTime;
            return;
        }
        
        _attackTimer = 0;
        
        Attack(_shootTransform,25);
    }
    private void MoveTurret(Vector3 _target)
    {
        var _direction = (_target - transform.position).normalized;
        var _lookRotation = Quaternion.LookRotation(_direction + _offset);
        _rotateTop.rotation = Quaternion.Slerp(_rotateTop.rotation, _lookRotation, _rotateSpeed * Time.deltaTime);

        // Debugging
        Debug.DrawLine(transform.position, _target, Color.red);
    }
    public bool CheckHealth(float _value)
    {
        _health -= _value;
        if (!(_health <= 0)) return true;
        DestroyTurret();
        return false;
    }

    public void DestroyTurret()
    {
        Destroy(gameObject);
    }
    protected void Attack(Transform _bulletSpawn,int _bulletSpeed)
    {
        _bulletSpawn.LookAt(_player.transform.position);
        var _bulletInstance = Instantiate(_bullet, _bulletSpawn.position,
            _bulletSpawn.rotation);
        var _bulletScript = _bulletInstance.GetComponent<Bullet>();
        if (_bulletSpeed != 0)
        {
            _bulletScript._bulletSpeed = _bulletSpeed;
        }
        _bulletScript._enemyShoot = true;
        _bulletScript._bulletDamage = _damage;
        _audioSource.PlayOneShot(_shootAudioClip);
        Destroy(_bulletInstance, 5);
    }
}

