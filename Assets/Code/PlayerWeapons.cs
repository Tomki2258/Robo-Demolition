using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public float _damageMultipler;
    public List<WeaponClass> _weaponsInUse;
    public GameObject _bullet;
    public List<WeaponClass> _allWeapons;
    private PlayerMovement _playerMovement;
    [Header("Standard Gun")] public WeaponClass _standardGunClass;
    public float _standardMaxTimer;
    public float _bulletDamage;
    public float _standardCurrentTimer;
    public Transform _standardSpawner;
    public AudioSource _standardAudioSource;
    public ParticleSystem _standardGunParticles;
    [Header("Sniper Gun")] public WeaponClass _sniperGunClass;
    public float _sniperGunDamage;
    public float _sniperMaxTimer;
    public float _sniperCurrentTimer;
    public Transform _sniperSpawner;
    public AudioSource _sniperAudioSource;
    [Header("Machine Gun")] public WeaponClass _machineGunClass;
    public float _machineGunDamage;
    public float _machineGunMaxTimer;
    public float _machineGunCurrentTimer;
    public Transform _machineGunSpawner;
    public AudioSource _machineGunAudioSource;

    [Header("Shotgun Gun")] public WeaponClass _shotgunGunClass;

    public float _shotgunMaxTimer;

    public Transform _shotgunSpawner;

    [Header("Circle Gun")] public WeaponClass _circleGunClass;

    public bool _circleGunEnabled;
    public float _circleGunMaxTimer;

    public int _shotsAmount;
    public Transform _circleGunSpawner;

    [Header("Sphere Attack")] public WeaponClass _sphereAttackClass;

    public bool _sphereAttackEnabled;
    public float _sphereAttackMaxTimer;
    public GameObject _sphereAttackPrefab;
    public float _sphereAttackCurrentTimer;

    [Header("Laser Gun")] public WeaponClass _laserGunClass;

    public bool _laserGunEnabled;
    public float _laserMaxTimer;
    public Transform _laserSpawner;
    public LineRenderer _lineRenderer;
    public float _laserCurrentDamage;
    public float _laserMaxDamage;
    public float _laserDamageMultiplier;
    public float _laserBaseDamage;
    public Transform _lastLaserEnemy;

    [Header("Rocket Launcher")] public WeaponClass _rocketLauncherClass;

    public float _rocketDamage;
    public bool _rocketLauncherEnabled;
    public GameObject _rocketPrefab;
    public float _rocketMaxTimer;
    public float _rocketCurrentTimer;
    public Transform _rocketSpawner;
    public List<GameObject> _weaponsModels;
    private float _circleGunCurrentTimer;
    private EquipmentCanvas _equipmentCanvas;
    private float _laserCurrentTimer;
    private float _shotgunCurrentTimer;
    public LayerMask _raycastIgnoreLayers;
    [Header("OrbitalGun")]
    public WeaponClass _orbitalGunClass;
    public float _orbitalGunMaxTimer;
    public float _orbitalGunCurrentTimer;
    public GameObject _orbitalGunPrefab;
    [Header("Mine deployer")]
    public WeaponClass _mineDeployerClass;
    public float _mineDeployerMaxTimer;
    public float _mineDeployerCurrentTimer;
    public GameObject _minePrefab;
    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _equipmentCanvas = FindFirstObjectByType<EquipmentCanvas>();
        _laserCurrentDamage = _laserBaseDamage;
        foreach (var _weapon in _weaponsModels) _weapon.SetActive(false);
    }

    public void SetWeaponsInUse()
    {
        foreach (var _weapon in _weaponsInUse)
            //Debug.LogWarning($"{_weapon.GetWeaponName()} is now in use !");
            _weapon.SetInUse(true);
    }

    public void StandardGun()
    {
        if (!_standardGunClass.CheckForUse()) return;
        if (_standardMaxTimer > _standardCurrentTimer)
        {
            _standardCurrentTimer += Time.deltaTime;
            return;
        }

        var _currentBullet = Instantiate(_bullet, _standardSpawner.transform.position, Quaternion.identity);
        _currentBullet.transform.rotation = _standardSpawner.transform.rotation;
        _currentBullet.GetComponent<Bullet>()._bulletDamage = _bulletDamage * _damageMultipler;
        _standardAudioSource.Play();
        _standardCurrentTimer = 0;
        if(_standardGunParticles != null) _standardGunParticles.Play();
    }

    public void MachineGun()
    {
        if (!_machineGunClass.CheckForUse())
            //Debug.LogWarning("Machine gun not unlocked !");
            return;
        if (_machineGunMaxTimer > _machineGunCurrentTimer)
        {
            _machineGunCurrentTimer += Time.deltaTime;
            return;
        }

        var _currentBullet = Instantiate(_bullet, _machineGunSpawner.transform.position, Quaternion.identity);
        _currentBullet.transform.rotation = _machineGunSpawner.transform.rotation;
        _currentBullet.GetComponent<Bullet>()._bulletDamage = _bulletDamage * _damageMultipler;
        _currentBullet.GetComponent<Bullet>()._bulletDamage = _machineGunDamage;
        _machineGunAudioSource.Play();
        _machineGunCurrentTimer = 0;
    }

    public void Sniper()
    {
        if (!_sniperGunClass.CheckForUse()) return;
        if (_sniperMaxTimer > _sniperCurrentTimer)
        {
            _sniperCurrentTimer += Time.deltaTime;
            return;
        }

        var _currentBullet = Instantiate(_bullet, _sniperSpawner.transform.position, Quaternion.identity);
        var _bulletScript = _currentBullet.GetComponent<Bullet>();
        _bulletScript._bulletDamage = _sniperGunDamage;
        _bulletScript._bulletSpeed = 50;
        _currentBullet.transform.rotation = _sniperSpawner.transform.rotation;
        _currentBullet.GetComponent<Bullet>()._bulletDamage = _bulletDamage * _damageMultipler;
//        _sniperAudioSource.Play();
        _sniperCurrentTimer = 0;
    }

    public void RocketLauncher()
    {
        if (!_rocketLauncherClass.CheckForUse()) return;
        if (_rocketMaxTimer > _rocketCurrentTimer)
        {
            _rocketCurrentTimer += Time.deltaTime;
            return;
        }

        var _currentRocket = Instantiate(_rocketPrefab, _rocketSpawner.position, _rocketSpawner.rotation);
        var _rocket = _currentRocket.GetComponent<Rocket>();
        _rocket._rocketDamage *= _damageMultipler;
        _rocketCurrentTimer = 0;
    }

    public void ShpereAttack()
    {
        if (!_sphereAttackClass.CheckForUse()) return;
        if (_sphereAttackCurrentTimer < _sphereAttackMaxTimer)
        {
            _sphereAttackCurrentTimer += Time.deltaTime;
            return;
        }

        var _currentSphere = Instantiate(_sphereAttackPrefab, transform.position, Quaternion.identity);
        _currentSphere.GetComponent<SphereAttack>()._player = GetComponent<PlayerMovement>();
        _sphereAttackCurrentTimer = 0;
    }

    public void ShotgunGun()
    {
        if (!_shotgunGunClass.CheckForUse()) return;
        if (_shotgunMaxTimer > _shotgunCurrentTimer)
        {
            _shotgunCurrentTimer += Time.deltaTime;
            return;
        }

        for (var i = 0; i < 3; i++)
        {
            var _currentBullet = Instantiate(_bullet, _shotgunSpawner.GetChild(i).position,
                _shotgunSpawner.GetChild(i).rotation);
            _currentBullet.GetComponent<Bullet>()._bulletDamage = _bulletDamage * _damageMultipler;
        }

        _shotgunCurrentTimer = 0;
    }

    public void CircleGun()
    {
        if (!_circleGunClass.CheckForUse()) return;
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
            _currentBullet.GetComponent<Bullet>()._bulletDamage = _bulletDamage * _damageMultipler;
        }

        _circleGunCurrentTimer = 0;
    }

    public void DoLaser(Transform _enemy)
    {
        if (!_laserGunClass.CheckForUse()) return;

        if (!_laserSpawner.gameObject.activeSelf)
        {
            _laserSpawner.gameObject.SetActive(true);
        }
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, _laserSpawner.position);
        _lineRenderer.SetPosition(1, _enemy.position);

        if (_enemy != _lastLaserEnemy) _laserCurrentDamage = _laserBaseDamage;

        if (_laserCurrentDamage < _laserMaxDamage)
            _laserCurrentDamage += _laserDamageMultiplier * Time.deltaTime * _damageMultipler;

        if (_laserMaxTimer > _laserCurrentTimer)
        {
            _laserCurrentTimer += Time.deltaTime;
        }
        else
        {
            var enemy = _enemy.GetComponent<Enemy>();
            enemy.CheckHealth(_laserCurrentDamage);
            _lastLaserEnemy = _enemy;
            _laserCurrentTimer = 0;
        }
    }

    public void DoOrbitalGun()
    {
        if(!_orbitalGunClass.CheckForUse()) return;
        
        if(_orbitalGunMaxTimer > _orbitalGunCurrentTimer)
        {
            _orbitalGunCurrentTimer += Time.deltaTime;
            
        }
        else
        {
            GameObject _orbitalGun = Instantiate(_orbitalGunPrefab,
                transform.position,
                Quaternion.identity);
            _orbitalGun.transform.position = _playerMovement._currentEnemy.transform.position;
            _orbitalGunCurrentTimer = 0;
        }
    }
    public void DoMineDeployer()
    {
        if(!_mineDeployerClass.CheckForUse()) return;
        
        if(_mineDeployerMaxTimer > _mineDeployerCurrentTimer)
        {
            _mineDeployerCurrentTimer += Time.deltaTime;
            return;
        }
        GameObject _mine = Instantiate(_minePrefab,
           transform.position + new Vector3(0, 1, 0),
            Quaternion.identity);
        _mine.transform.rotation = new Quaternion(0, Random.Range(0,360), 0, 0);
        _mine.GetComponent<Rigidbody>().AddForce(_mine.transform.forward * 5, ForceMode.Impulse);
        
        _mineDeployerCurrentTimer = 0;
    }
    public void ModyfyDamage(float _value)
    {
        Debug.LogWarning(_value);
        _bulletDamage *= 1 + _value;
        _sniperGunDamage *= 1 + _value;
        _machineGunDamage *= 1 + _value;
        _rocketDamage *= 1 + _value;
        _laserBaseDamage *= 1 + _value;
    }

    public void ModyfyReloadSpeed(float _value)
    {
        Debug.LogWarning(_value);
        _standardMaxTimer *= 1 - _value;
        _sniperMaxTimer *= 1 - _value;
        _machineGunMaxTimer *= 1 - _value;
        _rocketMaxTimer *= 1 - _value;
        _laserMaxTimer *= 1 - _value;
        _circleGunMaxTimer *= 1 - _value;
        _sphereAttackMaxTimer *= 1 - _value;
        _shotgunMaxTimer *= 1 - _value;
    }

    public List<WeaponClass> GetWeapons()
    {
        return _weaponsInUse;
    }

    public void SetUsedWeapons(List<WeaponClass> _weapons)
    {
        _weaponsInUse = _weapons;
        _weaponsInUse.RemoveAll(s => s == null);
    }
}