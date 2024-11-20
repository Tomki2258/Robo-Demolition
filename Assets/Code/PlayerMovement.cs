using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    public GameObject _inputCanvas;
    public float _speed;
    public float _rotationSpeed;
    public Transform _top;
    public Transform _legs;
    public Transform _hands;
    public int _turretRotateSpeed;
    public GameObject _currentEnemy;
    public float _attackRange; //base
    public float _currentAttackRange;
    public PlayerSkinSO _currentPlayerSkin;
    [Header("Player Stats")] public float _maxHealth;

    public float _health;
    public float _hpRegenMultipler;
    public int _xp;
    public int _level;
    public int _xpToNextLevel = 100;
    public bool _died;
    public bool _shield;
    public float _shieldTimer;
    public float _shieldMaxTimer;
    public int _shieldCount;
    public GameObject _shieldEffect;
    public PlayerWeapons _playerWeapons;
    public List<int> _weaponsUnlockStages;
    public int _activeWeaponModels;
    public List<GameObject> _weaponsModels;
    public Material _blackMaterial;
    public CameraShake _cameraShake;

    [Header("Hp UI")] public Slider _hpSlider;

    [SerializeField] public TMP_Text _hpText;

    [Header("XP UI")] public TMP_Text _xpText;

    public TMP_Text _xPProgressText;
    public Slider _xpSlider;
    public Animator _topAninmator;
    public float _maxFootStepTimer;
    public AudioSource _legsAudioSource;
    [Header("FootSteps sounds")] public List<AudioClip> _footStepSounds;
    private readonly int _hpRegenTime = 1;
    private Animator _animator;
    private CameraController _cameraController;
    private CharacterController _controller;
    private float _currentFootStepTimer;
    private int _footStepsSoundsCount;

    private bool _footWasLeft;
    private GameManager _gameManager;
    private float _hpRegenTimer;
    private bool _isJoystick;
    private VariableJoystick _joystick;
    private Animator _legsAnimator;
    private GameObject _nearestEnemy;
    public PlayerAtributtes _playerAtributtes;
    private PlayerDemolition _playerDemolition;
    private bool _playerMoving = false;
    private readonly List<Vector3> _startPlayerPositions = new(3);
    private readonly List<Quaternion> _startPlayerQuaterions = new(2);
    private UIManager _uiManager;
    public Transform _idleLookTransform;
    public float _levelUpPlayerScaler;
    private EquipmentCanvas _equipmentCanvas;
    private float _startPlayerY = 1.871f;
    private float _currentEnemyDist = float.MaxValue;
    private void Awake()
    {
        _equipmentCanvas = FindAnyObjectByType<EquipmentCanvas>();
        _footStepsSoundsCount = _footStepSounds.Count;
        _legsAnimator = _legs.GetComponent<Animator>();
        _startPlayerPositions.Add(_top.localPosition);
        _startPlayerQuaterions.Add(_top.localRotation);
        _startPlayerPositions.Add(_legs.localPosition);
        _startPlayerQuaterions.Add(_legs.localRotation);
        _startPlayerQuaterions.Add(_hands.localRotation);
        _playerDemolition = GetComponent<PlayerDemolition>();
        _playerDemolition.UpdatePlayerSize();
        SetUiValues();
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
        _animator.enabled = false;
        _cameraShake = _cameraController.gameObject.GetComponent<CameraShake>();
        _playerWeapons._laserSpawner.gameObject.SetActive(false);

        _inputCanvas.gameObject.SetActive(true);
        if(_gameManager._godMode) return;
        foreach (GameObject _weaponObject in _weaponsModels)
        {
            _weaponObject.SetActive(false);
        }
        
        _uiManager._shieldSlider.enabled = false;
        
        SetUiValues();
    }

    private void Update()
    {
        if (!_isJoystick || !_gameManager._gameLaunched) return;
        if (_joystick == null) _joystick = FindFirstObjectByType<VariableJoystick>();
        var _moveDirection = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);
        _controller.Move(_moveDirection * _speed * Time.deltaTime);

        if (CheckPlayerMove(_moveDirection))
        {
            _animator.SetBool("Moving", true);
            _legsAnimator.SetBool("Moving", true);
            _topAninmator.SetBool("Moving", true);
            Vector3 _targetRotation = Vector3.RotateTowards(_legs.parent.transform.forward,
                new Vector3(_moveDirection.x,
                    _moveDirection.y,
                    _moveDirection.z),
                _rotationSpeed * Time.deltaTime,
                0f);
            //_controller.transform.rotation = Quaternion.LookRotation(_targetRotation);
            _legs.parent.rotation = Quaternion.LookRotation(new Vector3(_targetRotation.x,
                _targetRotation.y,
                _targetRotation.z
                ));
            if (_currentEnemy == null)
            {
                //MoveTurret();
                //_playerWeapons._laserSpawner.gameObject.SetActive(false);
            }
            StepsSoundController();
        }
        else
        {
            _currentFootStepTimer = 0.1f;
            _animator.SetBool("Moving", false);
            _legsAnimator.SetBool("Moving", false);
            _topAninmator.SetBool("Moving", false);
        }
        ValidPlayerY();
    }
    public int GetRealAttackRange()
    {
        return Convert.ToInt16(_attackRange * (transform.localScale.x / 2) + 5);
    }
    private void FixedUpdate()
    {
        if (_died) return;
        
        HpRegeneration();
        ShieldManagment();
        //SetUiValues();
        
        if (_gameManager._spawnedEnemies.Count > 0)
        {
            var _nearestEnemy = GetNearestEnemy();
            _currentEnemyDist = Vector3.Distance(transform.position, _nearestEnemy.position);
            if (_playerWeapons._sniperGunClass.CheckInUse())
            {
                if (_currentEnemyDist < (GetRealAttackRange() * 1.75f)  
                    /*&& RaycastEnemy(_currentEnemy.transform) */)
                {
                    MoveTurret(_nearestEnemy.position);
                    Battle();
                }
            }
            else
            {
                if (_currentEnemyDist < GetRealAttackRange()  
                    /*&& RaycastEnemy(_currentEnemy.transform) */)
                {
                    MoveTurret(_nearestEnemy.position);
                    Battle();
                }
                else
                {
                    MoveTurret(_idleLookTransform.position);
                    _playerWeapons._laserSpawner.gameObject.SetActive(false);
                }    
            }
            if (_nearestEnemy == null)
                return;
        }
        else
        {
            MoveTurret(_idleLookTransform.position);
        }
    }

    private bool CheckPlayerMove(Vector3 _moveDirection)
    {
        return !(_moveDirection.sqrMagnitude <= 0);
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

    public void InstantLvlUp()
    {
        int _xpToLevel = _xpToNextLevel - _xp;
        AddPlayerXP(_xpToLevel + 1);
    }

    public void ActiveShield()
    {
        if(_shieldCount <= 0) return;
        
        if (_shield) return;
        
        _shieldCount--;
        _shield = true;
        _uiManager._shieldSlider.gameObject.SetActive(true);
    }
    private void ShieldManagment()
    {
        if (!_shield) return;

        if (_shieldTimer < _shieldMaxTimer)
        {
            _shieldEffect.SetActive(true);
            _shieldTimer += Time.deltaTime;
            
            _uiManager._shieldSlider.value = (_shieldMaxTimer - _shieldTimer);
        }
        else
        {
            _shieldTimer = 0;
            _shieldEffect.SetActive(false);
            _shield = false;
            _uiManager.ManageShieldButton();
            _uiManager._shieldSlider.gameObject.SetActive(false);
        }
    }
    
    private void MoveTurret(Vector3 _target)
    {
        var _direction = _target - transform.position;
        _direction.Normalize();
        _top.rotation = Quaternion.Slerp(_top.rotation, Quaternion.LookRotation(_direction),
            _turretRotateSpeed * Time.deltaTime);
    }

    private void Battle()
    {
        _playerWeapons.WeaponsReloads();
        
        _currentAttackRange = _attackRange * transform.localScale.x;

        if (_currentEnemyDist < _currentAttackRange)
        {
            _playerWeapons.StandardGun();
            _playerWeapons.ShotgunGun();
            _playerWeapons.CircleGun();
            _playerWeapons.MachineGun();
            _playerWeapons.ShpereAttack();
            _playerWeapons.RocketLauncher();
            _playerWeapons.DoLaser(_currentEnemy.transform);
            _playerWeapons.DoOrbitalGun();
            _playerWeapons.DoGranadeLauncher();
        }
        else
        {
            _playerWeapons._laserSpawner.gameObject.SetActive(false);
        }

        if (_currentEnemyDist < _currentAttackRange * 1.75f)
        {
            _playerWeapons.Sniper();
        }
        _playerWeapons.DoMineDeployer();
    }

    public void AddPlayerXP(int value)
    {
        int _finalValue = value;
        bool _isLucky = _playerAtributtes.PlayerLuck();
        if(_isLucky)
            _finalValue *= 2;
        _xp += _finalValue;
        XpManagment();
        SetUiValues();
        _uiManager.ShowXPDifference(_finalValue,_isLucky);
    }
    private void XpManagment()
    {
        if (_xp < _xpToNextLevel) return;
        
        _equipmentCanvas.CheckForWeaponPanels();
        _cameraShake.SwitchShakeMode(false);
        _uiManager.DoLevelUpCanvas(true);
        transform.position = new Vector3(transform.position.x,
            transform.position.y + 0.05f,
            transform.position.z);
        DoJoystickInput(false);
        
        _startPlayerY += 0.15f;
        _level++;
        _xp = 0;
        _xpToNextLevel += Convert.ToInt16(_xpToNextLevel * 0.3f);

        var _scale = transform.localScale;
        _scale += new Vector3(0.1f, 0.1f, 0.1f) * _levelUpPlayerScaler;
        transform.localScale = _scale;
        _maxHealth += Convert.ToInt16(_maxHealth * 0.1f);
        _health += Convert.ToInt16(_maxHealth * 0.10f);
        _speed+= 0.25f;
        if (_level % 3 == 0) _gameManager.IncreaseEnemiesIndex();

        _playerDemolition.UpdatePlayerSize();
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
        var _regenValue = _maxHealth * _hpRegenMultipler;
        _health += _regenValue;
        _uiManager.ShowHpDifference(_regenValue);
        SetUiValues();
    }

    public void DoJoystickInput(bool _mode)
    {
        _isJoystick = _mode;
        if(!_mode)
            _inputCanvas.transform.GetChild(0).gameObject.SetActive(_mode);
    }

    private bool RaycastEnemy(Transform enemy)
    {
        RaycastHit _hit;
        if (Physics.Raycast(transform.position, enemy.position - transform.position
                , out _hit, _playerWeapons._raycastIgnoreLayers))
        {
            if (_hit.transform.CompareTag("Enemy"))
            {
                return true;
            }
        }

        return false;
    }
    private Transform GetNearestEnemy()
    {
        var lowestDist = Mathf.Infinity;
        var enemiesList = _gameManager._spawnedEnemies;
        int enemyCount = enemiesList.Count;

        if (enemyCount == 0) return null;

        Vector3 currentPosition = transform.position;

        for (int i = 0; i < enemyCount; i++)
        {
            var dist = (enemiesList[i].transform.position - currentPosition).sqrMagnitude;

            if (dist < lowestDist)
            {
                lowestDist = dist;
                _currentEnemy = enemiesList[i];
            }
        }

        return _currentEnemy.transform;
    }


    public void CheckHealth(float _value)
    {
        if (_died) return;

        if (_shield)
        {
            _uiManager.ShowHpDifference(-2);
            return;
        }
        
        if (!_playerAtributtes.BulletReflection())
        {
            //NOT IMPLEMENTED YET
        }
        
        if (!_playerAtributtes.BulletDodge())
        {
            _uiManager.ShowHpDifference(-_value);
            _health -= _value;
            if (_health <= 0)
                Die();
        }
        else _uiManager.ShowHpDifference(-1);
        
        SetUiValues();
    }
    private void Die()
    {
        DoJoystickInput(false);
        _uiManager.EnableDieCanvas(true);
        _died = true;
        _cameraController._offset.y = 15;
        _cameraController._speed /= 2;
        _animator.enabled = true;
        _animator.SetTrigger("die");
        _legsAnimator.SetBool("Moving", false);
        _topAninmator.SetBool("Moving", false);
        _topAninmator.enabled = false;
        _shield = false;
        _shieldEffect.SetActive(false);
        UserData _userData = FindObjectOfType<UserData>();
        
        _userData.AddDeathCount();
        Debug.LogWarning(_userData.GetDeathCount());

        if (_userData.GetDeathCount() % 5 == 0)
        {
            if (PlayerPrefs.GetInt("AlreadyRated") == 0 && PlayerPrefs.GetInt("CanceledRate") < 3)
            {
                _uiManager._rateAppCanvas.SetActive(true);
            }    
        }
    }

    public void Revive()
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
            _cameraController._speed *= 2;
            _animator.enabled = false;
            _topAninmator.enabled = true;
    }

    public void DiePlayerTexture()
    {
        _cameraShake.DoShake(.15f, .5f);
        foreach (var _child in transform.GetComponentsInChildren<Transform>())
        {
            //Debug.LogWarning(_child.name);
            if (_child.GetComponent<MeshRenderer>())
            {
                var _meshRenderer = _child.GetComponent<MeshRenderer>();
                _meshRenderer.material = _blackMaterial;
            }else if (_child.GetComponent<SkinnedMeshRenderer>())
            {
                var _meshRenderer = _child.GetComponent<SkinnedMeshRenderer>();
                _meshRenderer.material = _blackMaterial;
            }
        }
    }

    private void StepsSoundController()
    {
        GameObject _effectInsstance = null;
        if (_currentFootStepTimer > _maxFootStepTimer)
        {
            _legsAudioSource.resource = _footStepSounds[Random.Range(0, _footStepsSoundsCount)];
            _legsAudioSource.Play();
            _currentFootStepTimer = 0;
        }
        else
        {
            _currentFootStepTimer += Time.deltaTime;
        }
    }

    private void ValidPlayerY()
    {
        Vector3 _currentVectr = transform.position;
        if (_currentVectr.y != _startPlayerY)
        {
            transform.position = new Vector3(_currentVectr.x,
                _startPlayerY,
                _currentVectr.z);
        }
    }
}