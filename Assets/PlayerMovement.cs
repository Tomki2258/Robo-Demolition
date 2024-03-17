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
    [Header("Standard Gun")] 
    public float _standardMaxTimer;
    private float _standardCurrentTimer;
    public Transform _standardSpawner;
    [Header("Shotgub Gun")] 
    public float _shotgunMaxTimer;
    private float _shotgunCurrentTimer;
    public Transform _shotgunSpawner;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _joystick = FindObjectOfType<VariableJoystick>();
        _gameManager = FindAnyObjectByType<GameManager>();
        EnableJoystickInput();
    }

    private void EnableJoystickInput()
    {
        _isJoystick = true;
        _inputCanvas.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if(_gameManager._spawnedEnemies.Count > 0)
        {
            GetNearestEnemy();
            _top.LookAt(_currentEnemy.transform.position);
        }
        StandardGun();
    }

    private void StandardGun()
    {
        if(_standardMaxTimer > _standardCurrentTimer)
        {
            _standardCurrentTimer += Time.deltaTime; 
            return;
        }

        GameObject _currentBullet = Instantiate(_bullet, _standardSpawner.transform.position, Quaternion.identity);
        _bullet.transform.rotation = _standardSpawner.transform.rotation;
        _standardCurrentTimer = 0;
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
}
