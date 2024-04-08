using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public GameObject _bullet;
    public float _bulletDamage;
    private float _circleGunCurrentTimer;

    [Header("Standard Gun")] public float _standardMaxTimer;
    public float _standardCurrentTimer;
    public Transform _standardSpawner;

    [Header("Shotgub Gun")] public bool _shotgunEnabled;
    public float _shotgunMaxTimer;

    public Transform _shotgunSpawner;

    [Header("Circle Gun")] public bool _circleGunEnabled;
    public float _circleGunMaxTimer;

    public int _shotsAmount;
    public Transform _circleGunSpawner;
    [Header("Sphere Attack")] public bool _sphereAttackEnabled;
    public float _sphereAttackMaxTimer; 
    public GameObject _sphereAttackPrefab;
    public float _sphereAttackCurrentTimer;
    private float _shotgunCurrentTimer;
    [Header("Laser Gun")] public bool _laserGunEnabled;
    public float _laserMaxTimer;
    public Transform _laserSpawner;
    private float _laserCurrentTimer;
    public LineRenderer _lineRenderer;
    public float _laserCurrentDamage;
    public float _laserMaxDamage;
    public float _laserDamageMultiplier;
    public float _laserBaseDamage;
    public Transform _lastLaserEnemy;
    [Header("Rocket Launcher")]
    public bool _rocketLauncherEnabled;
    public GameObject _rocketPrefab;
    public float _rocketMaxTimer;
    public float _rocketCurrentTimer;
    public Transform _rocketSpawner;
    public List<GameObject> _weaponsModels;

    private void Start()
    {
        _laserCurrentDamage = _laserBaseDamage;
        foreach (GameObject _weapon in _weaponsModels)
        {
            _weapon.SetActive(false);
        }
    }
    public void StandardGun()
    {
        if (_standardMaxTimer == 0) return;
        if (_standardMaxTimer > _standardCurrentTimer)
        {
            _standardCurrentTimer += Time.deltaTime;
            return;
        }

        var _currentBullet = Instantiate(_bullet, _standardSpawner.transform.position, Quaternion.identity);
        _currentBullet.transform.rotation = _standardSpawner.transform.rotation;
        _currentBullet.GetComponent<Bullet>()._bulletDamage = _bulletDamage;
        _standardCurrentTimer = 0;
    }
    public void RocketLauncher()
    {
        if (!_rocketLauncherEnabled) return;
        if (_rocketMaxTimer > _rocketCurrentTimer)
        {
            _rocketCurrentTimer += Time.deltaTime;
            return;
        }

        var _currentRocket = Instantiate(_rocketPrefab, _rocketSpawner.position, _rocketSpawner.rotation);
        _rocketCurrentTimer = 0;
    }
    public void ShpereAttack()
    {
        if (!_sphereAttackEnabled) return;
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
        if(!_shotgunEnabled) return;
        if (_shotgunMaxTimer > _shotgunCurrentTimer)
        {
            _shotgunCurrentTimer += Time.deltaTime;
            return;
        }

        for (var i = 0; i < 3; i++)
        {
            var _currentBullet = Instantiate(_bullet, _shotgunSpawner.GetChild(i).position,
                _shotgunSpawner.GetChild(i).rotation);
            _currentBullet.GetComponent<Bullet>()._bulletDamage = _bulletDamage;
        }

        _shotgunCurrentTimer = 0;
    }

    public void CircleGun()
    {
        if(!_circleGunEnabled) return;
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
            _currentBullet.GetComponent<Bullet>()._bulletDamage = _bulletDamage;
        }

        _circleGunCurrentTimer = 0;
    }
    public void DoLaser(Transform _enemy)
    {
        if(!_laserGunEnabled) return;
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0,_laserSpawner.position);
        _lineRenderer.SetPosition(1,_enemy.position);
        
        if (_enemy != _lastLaserEnemy)
        {
            _laserCurrentDamage = _laserBaseDamage;
        }
        
        if(_laserCurrentDamage < _laserMaxDamage)
        {
            _laserCurrentDamage += _laserDamageMultiplier * Time.deltaTime;
        }
        
        if (_laserMaxTimer > _laserCurrentTimer)
        {
            _laserCurrentTimer += Time.deltaTime;
        }
        else
        {
            Enemy enemy = _enemy.GetComponent<Enemy>();
            enemy.CheckHealth(_laserCurrentDamage);
            _lastLaserEnemy = _enemy;
            _laserCurrentTimer = 0;
        }
    }
}
