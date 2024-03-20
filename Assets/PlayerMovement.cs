using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Canvas _inputCanvas;
    public float _speed;
    public float _rotationSpeed;
    public Transform _top;
    public GameObject _currentEnemy;
    public GameObject _bullet;
    public int _attackRange;
    public int _damage;

    [Header("Standard Gun")] public float _standardMaxTimer;

    public Transform _standardSpawner;

    [Header("Shotgub Gun")] public float _shotgunMaxTimer;

    public Transform _shotgunSpawner;

    [Header("Circle Gun")] public float _circleGunMaxTimer;

    public int _shotsAmount;
    public Transform _circleGunSpawner;
    [Header("Sphere Attack")]
    public float _sphereAttackMaxTimer;
    // ide się wylać, skończ to na obiektówece 
    [Header("Player Stats")] public int _maxHealth;
    
    public float _health;
    private int _hpRegenTime = 1;
    private float _hpRegenTimer;
    public int _xp;
    public int _level;
    public int _xpToNextLevel = 100;
    private float _circleGunCurrentTimer;
    private CharacterController _controller;
    private GameManager _gameManager;
    private bool _isJoystick;
    private VariableJoystick _joystick;
    private GameObject _nearestEnemy;
    private float _shotgunCurrentTimer;
    private float _standardCurrentTimer;
    

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _joystick = FindObjectOfType<VariableJoystick>();
        _gameManager = FindAnyObjectByType<GameManager>();
        _health = _maxHealth;
        EnableJoystickInput();
    }

    private void Update()
    {
        if (!_isJoystick) return;
        var _moveDirection = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);
        _controller.Move(_moveDirection * _speed * Time.deltaTime);

        if (_moveDirection.sqrMagnitude <= 0) return;

        var _targetRotation = Vector3.RotateTowards(_controller.transform.forward,
            _moveDirection,
            _rotationSpeed * Time.deltaTime,
            0f);
        _controller.transform.rotation = Quaternion.LookRotation(_targetRotation);
    }

    private void FixedUpdate()
    {
        if (_gameManager._spawnedEnemies.Count <= 0) return;

        GetNearestEnemy();
        var _direction = _currentEnemy.transform.position - transform.position;
        _direction.Normalize();
        _top.rotation = Quaternion.Slerp(_top.rotation, Quaternion.LookRotation(_direction), 10 * Time.deltaTime);
        var _enemyDist = Vector3.Distance(transform.position, _currentEnemy.transform.position);
        if (_enemyDist < _attackRange)
        {
            //StandardGun();
            //ShotgunGun();
            //CircleGun();
        }
        HpRegeneration();
        XpManagment();
    }

    private void XpManagment()
    {
        if(_xp >= _xpToNextLevel)
        {
            _level++;
            _xp = 0;
            _xpToNextLevel += Convert.ToInt16(_xpToNextLevel * 0.5f);

            Vector3 _scale = transform.localScale;
            _scale += new Vector3(0.1f, 0.1f, 0.1f);
            transform.localScale = _scale;
            
            _maxHealth += Convert.ToInt16(_maxHealth * 0.15f);
            _health += Convert.ToInt16(_maxHealth * 0.15f);
        }
    }
    private void HpRegeneration()
    {
        if (_hpRegenTimer < _hpRegenTime)
        {
            _hpRegenTimer += Time.deltaTime;
            return;
        }
        
        if(_health >= _maxHealth) return;
        
        _hpRegenTimer = 0;
        _health += _maxHealth * 0.01f;
    }
    private void EnableJoystickInput()
    {
        _isJoystick = true;
        _inputCanvas.gameObject.SetActive(true);
    }

    private void StandardGun()
    {
        if (_standardMaxTimer == 0) return;
        if (_standardMaxTimer > _standardCurrentTimer)
        {
            _standardCurrentTimer += Time.deltaTime;
            return;
        }

        var _currentBullet = Instantiate(_bullet, _standardSpawner.transform.position, Quaternion.identity);
        _currentBullet.transform.rotation = _standardSpawner.transform.rotation;
        _standardCurrentTimer = 0;
    }
    private void ShpereAttack()
    {
        if (_sphereAttackMaxTimer == 0) return;
        if (_standardMaxTimer > _standardCurrentTimer)
        {
            _standardCurrentTimer += Time.deltaTime;
            return;
        }

        var _currentBullet = Instantiate(_bullet, _standardSpawner.transform.position, Quaternion.identity);
        _currentBullet.transform.rotation = _standardSpawner.transform.rotation;
        _standardCurrentTimer = 0;
    }
    private void ShotgunGun()
    {
        if(_shotgunMaxTimer == 0) return;
        if (_shotgunMaxTimer > _shotgunCurrentTimer)
        {
            _shotgunCurrentTimer += Time.deltaTime;
            return;
        }

        for (var i = 0; i < 3; i++)
        {
            var _currentBullet = Instantiate(_bullet, _shotgunSpawner.GetChild(i).position,
                _shotgunSpawner.GetChild(i).rotation);
        }

        _shotgunCurrentTimer = 0;
    }

    private void CircleGun()
    {
        if(_circleGunMaxTimer == 0) return;
        if (_circleGunMaxTimer > _circleGunCurrentTimer)
        {
            _circleGunCurrentTimer += Time.deltaTime;
            return;
        }

        Debug.Log("Circle Gun !");
        float _rotateValue = 360 / _shotsAmount;

        for (var i = 0; i < _shotsAmount; i++)
        {
            var _currentBullet =
                Instantiate(_bullet, _circleGunSpawner.GetChild(0).position, _circleGunSpawner.rotation);
            _circleGunSpawner.Rotate(0, _rotateValue, 0);
        }

        _circleGunCurrentTimer = 0;
    }

    private void GetNearestEnemy()
    {
        var lowestDist = Mathf.Infinity;
        foreach (var _enemy in _gameManager._spawnedEnemies)
        {
            var dist = Vector3.Distance(_enemy.transform.position, transform.position);

            if (dist < lowestDist)
            {
                lowestDist = dist;
                _currentEnemy = _enemy;
            }
        }
    }

    public void CheckHealth(int _value)
    {
        _health -= _value;
        if (_health <= 0) ;
        //Die();
    }

    private void Die()
    {
        Debug.Log("Player died !");
    }
}