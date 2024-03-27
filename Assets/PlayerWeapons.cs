using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public GameObject _bullet;
    private float _circleGunCurrentTimer;

    [Header("Standard Gun")] public float _standardMaxTimer;
    public Transform _standardSpawner;

    [Header("Shotgub Gun")] public float _shotgunMaxTimer;

    public Transform _shotgunSpawner;

    [Header("Circle Gun")] public float _circleGunMaxTimer;

    public int _shotsAmount;
    public Transform _circleGunSpawner;
    [Header("Sphere Attack")]
    public float _sphereAttackMaxTimer; 
    public GameObject _sphereAttackPrefab;
    public float _sphereAttackCurrentTimer;
    private float _shotgunCurrentTimer;
    private float _standardCurrentTimer;
    
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
        _standardCurrentTimer = 0;
    }
    public void ShpereAttack()
    {
        if (_sphereAttackMaxTimer == 0) return;
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

    public void CircleGun()
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
}
