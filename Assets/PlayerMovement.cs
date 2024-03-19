using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;
    private VariableJoystick _joystick;
    public Canvas _inputCanvas;
    private bool _isJoystick;
    public float _speed;
    public float _rotationSpeed;
    public Transform _top;
    private GameObject _nearestEnemy;
    private GameManager _gameManager;
    public GameObject _currentEnemy;
    public GameObject _bullet;
    public int _attackRange;
    public int _damage;
    [Header("Standard Gun")] 
    public float _standardMaxTimer;
    private float _standardCurrentTimer;
    public Transform _standardSpawner;
    [Header("Shotgub Gun")] 
    public float _shotgunMaxTimer;
    private float _shotgunCurrentTimer;
    public Transform _shotgunSpawner;
    [Header("Player Stats")] 
    public int _maxHealth;
    public int _health;
    public int _xp;
    public int _level;
    

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _joystick = FindObjectOfType<VariableJoystick>();
        _gameManager = FindAnyObjectByType<GameManager>();
        _health = _maxHealth;
        EnableJoystickInput();
    }

    private void EnableJoystickInput()
    {
        _isJoystick = true;
        _inputCanvas.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if(_gameManager._spawnedEnemies.Count <= 0)
        {
            return;
        }
        
        GetNearestEnemy();
        Vector3 _direction = _currentEnemy.transform.position - transform.position;
        _direction.Normalize();
        _top.rotation = Quaternion.Slerp(_top.rotation, Quaternion.LookRotation(_direction), 10 * Time.deltaTime);
        float _enemyDist = Vector3.Distance(transform.position, _currentEnemy.transform.position);
        if (_enemyDist < _attackRange)
        {
            StandardGun();
            ShotgunGun();
        }
    }

    private void StandardGun()
    {
        if(_standardMaxTimer > _standardCurrentTimer)
        {
            _standardCurrentTimer += Time.deltaTime; 
            return;
        }

        GameObject _currentBullet = Instantiate(_bullet, _standardSpawner.transform.position, Quaternion.identity);
        _currentBullet.transform.rotation = _standardSpawner.transform.rotation;
        _standardCurrentTimer = 0;
    }
    private void ShotgunGun()
    {
        if(_shotgunMaxTimer > _shotgunCurrentTimer)
        {
            _shotgunCurrentTimer += Time.deltaTime; 
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject _currentBullet = Instantiate(_bullet, _shotgunSpawner.GetChild(i).position, _shotgunSpawner.GetChild(i).rotation);
        }
        _shotgunCurrentTimer = 0;
    }
    private void GetNearestEnemy()
    {
        float lowestDist = Mathf.Infinity;
        foreach(GameObject _enemy in _gameManager._spawnedEnemies)
        {
 
            float dist = Vector3.Distance(_enemy.transform.position, transform.position);
 
            if (dist<lowestDist)
            {
                lowestDist = dist;
                _currentEnemy = _enemy;
            }
        }
    }
    private void Update()
    {
        if(!_isJoystick) return;
        var _moveDirection = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);
        _controller.Move(_moveDirection * _speed * Time.deltaTime);
        
        if(_moveDirection.sqrMagnitude <= 0) return;

        var _targetRotation = Vector3.RotateTowards(_controller.transform.forward,
            _moveDirection,
            _rotationSpeed * Time.deltaTime,
            0f);
        _controller.transform.rotation = Quaternion.LookRotation(_targetRotation);
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
