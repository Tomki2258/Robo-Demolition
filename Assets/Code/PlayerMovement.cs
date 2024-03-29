using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject _inputCanvas;
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
    public GameObject _shieldEffect;
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
    [SerializeField] private List<int> _weaponsUnlockStages;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _joystick = FindFirstObjectByType<VariableJoystick>();
        _gameManager = FindAnyObjectByType<GameManager>();
        _health = _maxHealth;
        _cameraController = FindFirstObjectByType<CameraController>();
        _uiManager = FindAnyObjectByType<UIManager>();
        DoJoystickInput(true);
        _playerWeapons = GetComponent<PlayerWeapons>();
        _shieldEffect.SetActive(false);
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
        if(_currentEnemy == null)
            //MoveTurret();
            _playerWeapons._laserSpawner.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_died) return;

        // if(_health <= 0)
        //     Die();
        XpManagment();
        HpRegeneration();
        ShieldManagment();
        
        if (_gameManager._spawnedEnemies.Count <= 0) return;
        MoveTurret();
        Battle();
    }

    private void ShieldManagment()
    {
        if (!_shield) return;

        if (_shieldTimer < _shieldMaxTimer)
        {
            _shield = true;
            _shieldEffect.SetActive(true);
            _shieldTimer += Time.deltaTime;
        }
        else
        {
            _shieldTimer = 0;
            _shieldEffect.SetActive(false);
            _shield = false;
        }
    }

    private void CheckForWeaponUnlock(int _currentLevel)
    {
        if (_weaponsUnlockStages.Contains(_currentLevel))
        {
            int _index = _weaponsUnlockStages.IndexOf(_currentLevel);
        
            switch (_index)
            {
                case 0:
                    _playerWeapons._shotgunEnabled = true;
                    _playerWeapons._weaponsModels[0].SetActive(true);
                    _uiManager.UnlockWeaponUI("Shotgun", null);
                    break;
                case 1:
                    _playerWeapons._circleGunEnabled = true;
                    _playerWeapons._weaponsModels[1].SetActive(true);
                    _uiManager.UnlockWeaponUI("Circle gun", null);

                    break;
                case 2:
                    _playerWeapons._sphereAttackEnabled = true;
                    _playerWeapons._weaponsModels[2].SetActive(true);
                    _uiManager.UnlockWeaponUI("Sphere bomb", null);

                    break;
                case 3:
                    _playerWeapons._laserGunEnabled = true;
                    _playerWeapons._weaponsModels[3].SetActive(true);
                    _uiManager.UnlockWeaponUI("Laser", null);
                    break;
                case 4:
                    _playerWeapons._rocketLauncherEnabled = true;
                    _playerWeapons._weaponsModels[4].SetActive(true);
                    _uiManager.UnlockWeaponUI("Rocket launcher", null);
                    break;
            }
        }
    }
    private void MoveTurret()
    {
        var _direction = GetNearestEnemy().transform.position - transform.position;
        _direction.Normalize();
        _top.rotation = Quaternion.Slerp(_top.rotation, Quaternion.LookRotation(_direction), 10 * Time.deltaTime);
    }

    private void Battle()
    {
        var _enemyDist = Vector3.Distance(transform.position, _currentEnemy.transform.position);
        float _tempAttackRange = _attackRange * transform.localScale.x;
        if (_enemyDist < _tempAttackRange)
        {
            _playerWeapons.StandardGun();
            _playerWeapons.ShotgunGun();
            _playerWeapons.CircleGun();
            _playerWeapons.ShpereAttack(); _playerWeapons.RocketLauncher(); 
            _playerWeapons._laserSpawner.gameObject.SetActive(true);
            _playerWeapons.DoLaser(_currentEnemy.transform);
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

            _uiManager.DoLevelUpCanvas(true);
            transform.position = new Vector3(transform.position.x,
                transform.position.y + 0.05f,
                transform.position.z);
            DoJoystickInput(false);
            CheckForWeaponUnlock(_level);
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

    public void DoJoystickInput(bool _mode)
    {
        _isJoystick = _mode;
        _inputCanvas.gameObject.SetActive(_mode);
    }

    private Transform GetNearestEnemy()
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

        return _currentEnemy.transform;
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
        _uiManager.EnableDieCanvas();
        _died = true;
        _cameraController._offset.y -= 5;
    }
}