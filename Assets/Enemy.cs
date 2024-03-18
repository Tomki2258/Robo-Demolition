using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _agent;
    public GameManager _gameManager;
    public PlayerMovement _player;
    public List<GameObject> _lights;
    private float _lightsTimerMax = 1;
    private float _lightsTimer;
    private int health = 10;
    public GameObject _explosionPrefab;
    private CameraShake _cameraShake;
    public State _currentState;
    public int _attackRange;
    public int _incomingRange;
    public int _xpReward;
    public enum State
    {
        Incoming,
        Attacking
    }
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _cameraShake = FindAnyObjectByType<CameraShake>();
        _currentState = State.Incoming;
    }

    private void FixedUpdate()
    {
        //SwitchLight();
        
        float _distance = Vector3.Distance(transform.position, _player.transform.position);
        if (_currentState == State.Incoming)
        {
            if (_distance < _attackRange)
                _currentState = State.Attacking;
            _agent.SetDestination(_player.transform.position);
            _agent.isStopped = false;
        }
        else if (_currentState == State.Attacking)
        {
            if(_distance > _incomingRange)
                _currentState = State.Incoming;
            _agent.isStopped = true;
        }
    }

    public void CheckHealth(int _value)
    {
        health -= _value;
        if(health <= 0)
            Die();
    }
    private void Die()
    {
        Debug.Log("Enemy died");
        _gameManager.RemoveEnemy(gameObject);
        GameObject _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _cameraShake.DoShake(0.001f, 1);
        _player._xp += _xpReward;
        Destroy(_explosion,5);
        Destroy(gameObject);
    }

    private void SwitchLight()
    {
        if (_lightsTimerMax > _lightsTimer)
        {
            _lightsTimer += Time.deltaTime;
        }
        else
        {
            _lightsTimer = 0;
            _lights[0].SetActive(_lights[0].activeSelf);
            _lights[1].SetActive(!_lights[1].activeSelf);
        }
    }
}
