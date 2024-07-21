using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public float _damageMultipler;
    public float _reloadMultipler;
    public List<WeaponClass> _weaponsInUse;
    public GameObject _bullet;
    public List<WeaponClass> _allWeapons;
    private PlayerMovement _playerMovement;
    [Header("Standard Gun")] public WeaponClass _standardGunClass;
    public float _standardCurrentTimer;
    public Transform _standardSpawner;
    public AudioSource _standardAudioSource;
    public ParticleSystem _standardGunParticles;
    [Header("Sniper Gun")] public WeaponClass _sniperGunClass;
    public float _sniperCurrentTimer;
    public Transform _sniperSpawner;
    public AudioSource _sniperAudioSource;
    [Header("Machine Gun")] public WeaponClass _machineGunClass;
    [Range(0, 10)] public float _machineGunRecoil;
    public float _machineGunCurrentTimer;
    public Transform _machineGunSpawner;
    public AudioSource _machineGunAudioSource;

    [Header("Shotgun Gun")] public WeaponClass _shotgunGunClass;
    public int _pelletAngle;
    public int _shotGunPellets;
    public Transform _shotgunSpawner;
    public float _shotgunCurrentTimer;

    [Header("Circle Gun")] public WeaponClass _circleGunClass;
    
    public int _shotsAmount;
    public Transform _circleGunSpawner;
    public float _circleGunCurrentTimer;

    [Header("Sphere Attack")] public WeaponClass _sphereAttackClass;

    public GameObject _sphereAttackPrefab;
    public float _sphereAttackCurrentTimer;

    [Header("Laser Gun")] public WeaponClass _laserGunClass;

    public Transform _laserSpawner;
    public LineRenderer _lineRenderer;
    public float _laserCurrentDamage;
    public float _laserMaxDamage;
    public float _laserDamageMultiplier;
    public Transform _lastLaserEnemy;

    [Header("Rocket Launcher")] public WeaponClass _rocketLauncherClass;

    public GameObject _rocketPrefab;
    public float _rocketCurrentTimer;
    public Transform _rocketSpawner;
    public List<GameObject> _weaponsModels;
    private EquipmentCanvas _equipmentCanvas;
    private float _laserCurrentTimer;
    public LayerMask _raycastIgnoreLayers;
    [Header("OrbitalGun")]
    public WeaponClass _orbitalGunClass;
    public float _orbitalGunCurrentTimer;
    public GameObject _orbitalGunPrefab;
    [Header("Mine deployer")]
    public WeaponClass _mineDeployerClass;
    public float _mineDeployerCurrentTimer;
    public GameObject _minePrefab;
    [Header("Granade Launcher")]
    public WeaponClass _granadeLauncherClass;
    public float _granadeLauncherCurrentTimer;
    public GameObject _granadePrefab;
    public float _granadeForce;
    public Transform _granadeSpawner;
    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _equipmentCanvas = FindFirstObjectByType<EquipmentCanvas>();
        _laserCurrentDamage = _laserGunClass.GetDamage();
        foreach (var _weapon in _weaponsModels) _weapon.SetActive(false);
    }

    public void SetWeaponsInUse()
    {
        foreach (var _weapon in _weaponsInUse)
            _weapon.SetInUse(true);
    }

    private float GetFinalReloadTime(WeaponClass _weapon)
    {
        float _weaponsMultipler = 1 + (_equipmentCanvas.GetUsedWeapons() * 0.1f);
        return _weapon.GetReloadTime() * _reloadMultipler * _weaponsMultipler;
    }
    
    public void WeaponsReloads()
    {
        if (_standardGunClass.GetReloadTime() > _standardCurrentTimer)
        {
            _standardCurrentTimer += Time.deltaTime;
        } 
        if (GetFinalReloadTime(_machineGunClass) > _machineGunCurrentTimer)
        {
            _machineGunCurrentTimer += Time.deltaTime;
        }
        if (GetFinalReloadTime(_sniperGunClass) > _sniperCurrentTimer)
        {
            _sniperCurrentTimer += Time.deltaTime;
        }
        if (GetFinalReloadTime(_rocketLauncherClass) > _rocketCurrentTimer)
        {
            _rocketCurrentTimer += Time.deltaTime;
        }
        if (GetFinalReloadTime(_sphereAttackClass) > _sphereAttackCurrentTimer)
        {
            _sphereAttackCurrentTimer += Time.deltaTime;
        }
        if (GetFinalReloadTime(_shotgunGunClass) > _shotgunCurrentTimer)
        {
            _shotgunCurrentTimer += Time.deltaTime;
        }
        if (GetFinalReloadTime(_circleGunClass) > _circleGunCurrentTimer)
        {
            _circleGunCurrentTimer += Time.deltaTime;
        }
        if(GetFinalReloadTime(_mineDeployerClass) > _mineDeployerCurrentTimer)
        {
            _mineDeployerCurrentTimer += Time.deltaTime;
        }
        if(GetFinalReloadTime(_granadeLauncherClass) > _granadeLauncherCurrentTimer)
        {
            _granadeLauncherCurrentTimer += Time.deltaTime;
        }
    }
    public void StandardGun()
    {
        if (!_standardGunClass.CheckForUse()) return;
        if (_standardGunClass.GetReloadTime() > _standardCurrentTimer)
        {
            return;
        }

        var _currentBullet = Instantiate(_bullet,  _machineGunSpawner.transform.position, Quaternion.identity);
        _currentBullet.transform.rotation = _standardSpawner.transform.rotation;
        _currentBullet.GetComponent<Bullet>()._bulletDamage = _standardGunClass.GetDamage() * _damageMultipler;
        _standardAudioSource.Play();
        _standardCurrentTimer = 0;
        if(_standardGunParticles != null) _standardGunParticles.Play();
    }
    public void MachineGun()
    {
        if (!_machineGunClass.CheckForUse())
            //Debug.LogWarning("Machine gun not unlocked !");
            return;
        if (GetFinalReloadTime(_machineGunClass) > _machineGunCurrentTimer)
        {
            return;
        }

        var _currentBullet = Instantiate(_bullet, _machineGunSpawner.transform.position, Quaternion.identity);
        _currentBullet.transform.rotation = _machineGunSpawner.transform.rotation;
        _currentBullet.GetComponent<Bullet>().AddRecoil(_machineGunRecoil);
        _currentBullet.GetComponent<Bullet>()._bulletDamage = _machineGunClass.GetDamage() * _damageMultipler;
        _machineGunAudioSource.Play();
        _machineGunCurrentTimer = 0;
    }

    public void Sniper()
    {
        if (!_sniperGunClass.CheckForUse()) return;
        if (GetFinalReloadTime(_sniperGunClass) > _sniperCurrentTimer)
        {
            return;
        }

        var _currentBullet = Instantiate(_bullet, _sniperSpawner.transform.position, Quaternion.identity);
        var _bulletScript = _currentBullet.GetComponent<Bullet>();
        _bulletScript._bulletDamage = _shotgunGunClass.GetDamage();
        _bulletScript._bulletSpeed = 50;
        _currentBullet.transform.rotation = _sniperSpawner.transform.rotation;
        _currentBullet.GetComponent<Bullet>()._bulletDamage = _shotgunGunClass.GetDamage() * _damageMultipler;
//        _sniperAudioSource.Play();
        _sniperCurrentTimer = 0;
    }

    public void RocketLauncher()
    {
        if (!_rocketLauncherClass.CheckForUse()) return;
        if (GetFinalReloadTime(_rocketLauncherClass) > _rocketCurrentTimer)
        {
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
        
        if (GetFinalReloadTime(_sphereAttackClass) > _sphereAttackCurrentTimer)
        {
            return;
        }

        var _currentSphere = Instantiate(_sphereAttackPrefab, transform.position, Quaternion.identity);
        _currentSphere.GetComponent<SphereAttack>()._player = GetComponent<PlayerMovement>();
        _sphereAttackCurrentTimer = 0;
    }

    public void ShotgunGun()
    {
        if (!_shotgunGunClass.CheckForUse()) return;
        if (GetFinalReloadTime(_shotgunGunClass) > _shotgunCurrentTimer)
        {
            return;
        }

        bool _switchSide = false;
        int _srakaAngle = 0;
        for (var i = 0; i < _shotGunPellets; i++)
        {
            var _currentBullet = Instantiate(_bullet, _shotgunSpawner.position, _shotgunSpawner.rotation);
    
            int _currentAngle = 0;
            if (i % 2 == 0)
            {
                _currentAngle = (i / 2) * _pelletAngle; // Liczymy kąt tylko dla parzystych wartości i
            }
            else
            {
                _currentAngle = ((i + 1) / 2) * _pelletAngle; // Dla nieparzystych wartości i przesuwamy o jedną jednostkę
            }

            _currentAngle *= (_switchSide ? 1 : -1); // Zmieniamy znak kąta
            _switchSide = !_switchSide; // Przełączamy stronę po ustawieniu kąta

            _currentBullet.transform.Rotate(0, _currentAngle, 0);
            _currentBullet.GetComponent<Bullet>()._bulletDamage = _shotgunGunClass.GetDamage() * _damageMultipler;
        }


        _shotgunCurrentTimer = 0;
    }

    public void CircleGun()
    {
        if (!_circleGunClass.CheckForUse()) return;
        if (GetFinalReloadTime(_circleGunClass) > _circleGunCurrentTimer)
        {
            return;
        }

        float _rotateValue = 360 / _shotsAmount;

        for (var i = 0; i < _shotsAmount; i++)
        {
            var _currentBullet =
                Instantiate(_bullet, _circleGunSpawner.GetChild(0).position, _circleGunSpawner.rotation);
            _circleGunSpawner.Rotate(0, _rotateValue, 0);
            _currentBullet.GetComponent<Bullet>()._bulletDamage = _circleGunClass.GetDamage() * _damageMultipler;
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

        if (_enemy != _lastLaserEnemy) _laserCurrentDamage = _laserGunClass.GetDamage();

        if (_laserCurrentDamage < _laserMaxDamage)
            _laserCurrentDamage += _laserDamageMultiplier * Time.deltaTime * _damageMultipler;

        if (GetFinalReloadTime(_laserGunClass) > _laserCurrentTimer)
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
        
        if(GetFinalReloadTime(_orbitalGunClass) > _orbitalGunCurrentTimer)
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
        
        if(GetFinalReloadTime(_mineDeployerClass) > _mineDeployerCurrentTimer)
        {
            return;
        }
        GameObject _mine = Instantiate(_minePrefab,
           transform.position + new Vector3(0, 1, 0),
            Quaternion.identity);
        _mine.transform.rotation = new Quaternion(0, Random.Range(0,360), 0, 0);
        _mine.GetComponent<Rigidbody>().AddForce(_mine.transform.forward * 5, ForceMode.Impulse);
        
        _mineDeployerCurrentTimer = 0;
    }

    public void DoGranadeLauncher()
    {
        if(!_granadeLauncherClass.CheckForUse()) return;
        
        if(GetFinalReloadTime(_granadeLauncherClass) > _granadeLauncherCurrentTimer)
        {
            return;
        }
        GameObject _granade = Instantiate(_granadePrefab,
            _granadeSpawner.transform.position,
            Quaternion.identity);
        _granade.transform.rotation = _granadeSpawner.transform.rotation;
        
        _granade.GetComponent<Rigidbody>().AddForce(_granadeSpawner.transform.forward * 5, ForceMode.Impulse);
        _granadeLauncherCurrentTimer = 0;
    }
    public void ModyfyDamage(float _value)
    {
        Debug.LogWarning(_value);
        _damageMultipler *= 1 + _value;
    }

    public void ModyfyReloadSpeed(float _value)
    {
        Debug.LogWarning(_value);
        _reloadMultipler *= 1 - _value;
        /*
        _standardMaxTimer *= 1 - _value;
        _sniperMaxTimer *= 1 - _value;
        _machineGunMaxTimer *= 1 - _value;
        _rocketMaxTimer *= 1 - _value;
        _laserMaxTimer *= 1 - _value;
        _circleGunMaxTimer *= 1 - _value;
        _sphereAttackMaxTimer *= 1 - _value;
        _shotgunMaxTimer *= 1 - _value;
        */
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