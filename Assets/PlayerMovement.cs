using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Canvas _inputCanvas;
    public float _speed;
    public float _rotationSpeed;
    public Transform _top;
    public GameObject _currentEnemy;
    public float _attackRange;
    public int _damage;

    [Header("Player Stats")] public float _maxHealth;

    public float _health;
    public float _hpRegenMultipler = 0.1f;
    public int _xp;
    public int _level;
    public int _xpToNextLevel = 100;
    public bool _died;
    public bool _shield;
    public float _shieldTimer;
    public float _shieldMaxTimer;
    public PlayerWeapons _playerWeapons;
    private CameraController _cameraController;
    private CharacterController _controller;
    private GameManager _gameManager;
    private readonly int _hpRegenTime = 1;
    private float _hpRegenTimer;
    private bool _isJoystick;
    private VariableJoystick _joystick;
    private GameObject _nearestEnemy;
    private UIManager _uiManager;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _joystick = FindObjectOfType<VariableJoystick>();
        _gameManager = FindAnyObjectByType<GameManager>();
        _health = _maxHealth;
        _cameraController = FindObjectOfType<CameraController>();
        _uiManager = FindAnyObjectByType<UIManager>();
        DoJoystickInput(true);
        _playerWeapons = GetComponent<PlayerWeapons>();
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
        if (_died) return;

        if (_currentEnemy != null)
            MoveTurret();

        // if(_health <= 0)
        //     Die();

        if (_gameManager._spawnedEnemies.Count <= 0) return;
        GetNearestEnemy();
        HpRegeneration();
        XpManagment();
        ShieldManagment();
    }

    private void ShieldManagment()
    {
        if (!_shield) return;

        if (_shieldTimer < _shieldMaxTimer)
        {
            _shield = true;
            _shieldTimer += Time.deltaTime;
        }
        else
        {
            _shieldTimer = 0;
            _shield = false;
        }
    }

    private void MoveTurret()
    {
        var _direction = _currentEnemy.transform.position - transform.position;
        _direction.Normalize();
        _top.rotation = Quaternion.Slerp(_top.rotation, Quaternion.LookRotation(_direction), 10 * Time.deltaTime);
        var _enemyDist = Vector3.Distance(transform.position, _currentEnemy.transform.position);

        if (_enemyDist < _attackRange * transform.localScale.x)
        {
            _playerWeapons.StandardGun();
            _playerWeapons.ShotgunGun();
            _playerWeapons.CircleGun();
            _playerWeapons.ShpereAttack();
        }
    }

    private void XpManagment()
    {
        if (_xp >= _xpToNextLevel)
        {
            _level++;
            _xp = 0;
            _xpToNextLevel += Convert.ToInt16(_xpToNextLevel * 0.5f);

            var _scale = transform.localScale;
            _scale += new Vector3(0.1f, 0.1f, 0.1f);
            transform.localScale = _scale;
            _maxHealth += Convert.ToInt16(_maxHealth * 0.15f);
            _health += Convert.ToInt16(_maxHealth * 0.15f);

            _uiManager.DoLevelUpCanvas();
        }
    }

    private void HpRegeneration()
    {
        if (_health > _maxHealth) _health = _maxHealth;

        if (_hpRegenTimer < _hpRegenTime)
        {
            _hpRegenTimer += Time.deltaTime;
            return;
        }

        if (_health >= _maxHealth) return;

        _hpRegenTimer = 0;
        _health += _maxHealth * _hpRegenMultipler;
    }

    private void DoJoystickInput(bool _mode)
    {
        _isJoystick = _mode;
        _inputCanvas.gameObject.SetActive(_mode);
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

    public void CheckHealth(float _value)
    {
        if (_shield) return;

        _health -= _value;
        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        DoJoystickInput(false);
        _died = true;
        _cameraController._offset.y -= 5;
    }
}