using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public GameObject _inputCanvas;
    public float _speed;
    public float _rotationSpeed;
    public Transform _top;
    public Transform _legs;
    public Transform _hands;
    
    public GameObject _currentEnemy;
    public float _attackRange;
    public int _damage;
    private PlayerDemolition _playerDemolition;
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
    private Quaternion _startRotation;
    private Animator _animator;
    public Material _blackMaterial;
    public CameraShake _cameraShake;
    private List<Vector3> _startPlayerPositions = new List<Vector3>(3);
    private List<Quaternion> _startPlayerQuaterions = new List<Quaternion>(2);

    [Header("Hp UI")] public Slider _hpSlider;

    [SerializeField] public TMP_Text _hpText;

    [Header("XP UI")] public TMP_Text _xpText;

    public TMP_Text _xPProgressText;
    public Slider _xpSlider;
    private Quaternion _idleQuaterion;
    private PlayerAtributtes _playerAtributtes;
    private void Awake()
    {
        _idleQuaterion = new Quaternion(0, 0, 0, 0);
        _startPlayerPositions.Add(_top.localPosition);
        _startPlayerQuaterions.Add(_top.localRotation);
        _startPlayerPositions.Add(_legs.localPosition);
        _startPlayerQuaterions.Add(_legs.localRotation);    
        _startPlayerQuaterions.Add(_hands.localRotation);
        _playerDemolition = GetComponent<PlayerDemolition>();
        _playerDemolition.UpdatePlayerSize();
    }

    private void Start()
    {
        _playerAtributtes = GetComponent<PlayerAtributtes>();
        _controller = GetComponent<CharacterController>();
        _joystick = FindFirstObjectByType<VariableJoystick>();
        _gameManager = FindAnyObjectByType<GameManager>();
        _health = _maxHealth;
        _cameraController = FindFirstObjectByType<CameraController>();
        _uiManager = FindAnyObjectByType<UIManager>();
        DoJoystickInput(true);
        _playerWeapons = GetComponent<PlayerWeapons>();
        _shieldEffect.SetActive(false);
        _animator = GetComponent<Animator>();
        _startRotation = _top.rotation;
        _animator.enabled = false;
        _cameraShake = _cameraController.gameObject.GetComponent<CameraShake>();
    }

    private void Update()
    {
        if (!_isJoystick || !_gameManager._gameLaunched) return;
        if (_joystick == null)
        {
            _joystick = FindFirstObjectByType<VariableJoystick>();
        }
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
    private void SetUiValues()
    {
        _hpText.text = $"{Math.Round(_health)}" +
                       "/" +
                       $"{_maxHealth}";
        _hpSlider.value = _health;
        _hpSlider.maxValue = _maxHealth;

        _xpText.text = $"{_level}";

        _xpSlider.value = _xp;
        _xpSlider.maxValue = _xpToNextLevel;
        _xPProgressText.text = $"{_xp}" +
                               "/" +
                               $"{_xpToNextLevel}";
    }
    private void FixedUpdate()
    {
        if (_died) return;

        // if(_health <= 0)
        //     Die();
        XpManagment();
        HpRegeneration();
        ShieldManagment();
        SetUiValues();
        
        if (_gameManager._spawnedEnemies.Count > 0)
        {
            Transform _nearestEnemy = GetNearestEnemy();

            if (Vector3.Distance(transform.position, _nearestEnemy.position) < 20)
            {
                MoveTurret(GetNearestEnemy().position);
            }
            else
            {
                MoveTurret(transform.GetChild(transform.childCount - 1).position);
            }
            if(_nearestEnemy == null)
                return;
            Battle();
        }
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
                    _uiManager.UnlockUI("Shotgun", null);
                    break;
                case 1:
                    _playerWeapons._circleGunEnabled = true;
                    _playerWeapons._weaponsModels[1].SetActive(true);
                    _uiManager.UnlockUI("Circle gun", null);

                    break;
                case 2:
                    _playerWeapons._sphereAttackEnabled = true;
                    _playerWeapons._weaponsModels[2].SetActive(true);
                    _uiManager.UnlockUI("Sphere bomb", null);

                    break;
                case 3:
                    _playerWeapons._laserGunEnabled = true;
                    _playerWeapons._weaponsModels[3].SetActive(true);
                    _uiManager.UnlockUI("Laser", null);
                    break;
                case 4:
                    _playerWeapons._rocketLauncherEnabled = true;
                    _playerWeapons._weaponsModels[4].SetActive(true);
                    _uiManager.UnlockUI("Rocket launcher", null);
                    break;
            }
        }
    }
    private void MoveTurret(Vector3 _target)
    {
        var _direction = _target - transform.position;
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
            _playerWeapons.MachineGun();
            _playerWeapons.Sniper();
            _playerWeapons.ShpereAttack(); _playerWeapons.RocketLauncher(); 
            _playerWeapons.DoLaser(_currentEnemy.transform);
        }
    }

    private void XpManagment()
    {
        if (_xp >= _xpToNextLevel)
        {
            _uiManager.DoLevelUpCanvas(true);
            transform.position = new Vector3(transform.position.x,
                transform.position.y + 0.05f,
                transform.position.z);
            DoJoystickInput(false);
            CheckForWeaponUnlock(_level);
            
            _level++;
            _xp = 0;
            _xpToNextLevel += Convert.ToInt16(_xpToNextLevel * 0.5f);

            var _scale = transform.localScale;
            _scale += new Vector3(0.1f, 0.1f, 0.1f);
            transform.localScale = _scale;
            _maxHealth += Convert.ToInt16(_maxHealth * 0.15f);
            _health += Convert.ToInt16(_maxHealth * 0.15f);
            
            if (_level % 3 == 0)
            {
                _gameManager.IncreaseEnemiesIndex();
            }
            
            _playerDemolition.UpdatePlayerSize();
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
        float _regenValue = _maxHealth * _hpRegenMultipler;
        _health += _regenValue;
        _uiManager.ShowHpDifference(_regenValue);
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
            if(_enemy == null) continue;
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
        if (_shield || _died) return;
        
        if(_playerAtributtes.BulletDodge()) return;

        if (_playerAtributtes.BulletReflection())
        {
            //NOT IMPLEMENTED YET
        }
        
        _uiManager.ShowHpDifference(-_value);
        _health -= _value;
        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        DoJoystickInput(false);
        _uiManager.EnableDieCanvas();
        _died = true;
        _cameraController._offset.y = 7;
        _cameraController._speed /= 2;
        _cameraController._rotationSpeed *= 1.5f;
        _animator.enabled = true;
        _animator.SetTrigger("die");
        //_gameManager.DoAd();
    }

    public void Revive()
    {
        try
        {
            _top.localPosition = _startPlayerPositions[0];
            _top.localRotation = _startPlayerQuaterions[0];
            _legs.localPosition = _startPlayerPositions[1];
            _legs.localRotation = _startPlayerQuaterions[1];
            _hands.localRotation = _startPlayerQuaterions[2];
        
            _cameraController.SetOldOffset();
            _died = false;
            DoJoystickInput(true);
            _animator.SetTrigger("revive");
            _health = _maxHealth / 2;
            _uiManager.ShowHpDifference(_maxHealth / 2);
            _cameraController._offset.y = 15;
            _cameraController._speed *= 2;
            _animator.enabled = false;
        }
        catch (Exception e)
        {
            Debug.LogError($"Ten skrypt rzuca błędem:\n {e}");
            throw;
        }
    }

    public void DiePlayerTexture()
    {
        _cameraShake.DoShake(.15f, .5f);
        foreach (Transform _child in transform.GetComponentsInChildren<Transform>())
        {
            //Debug.LogWarning(_child.name);
            if (_child.GetComponent<MeshRenderer>())
            {
                MeshRenderer _meshRenderer = _child.GetComponent<MeshRenderer>();
                _meshRenderer.material = _blackMaterial;
            }
        }
    }
}