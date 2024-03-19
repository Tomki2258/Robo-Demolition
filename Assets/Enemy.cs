using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent _agent;
    public GameManager _gameManager;
    public PlayerMovement _player;
    public List<GameObject> _lights;
    private float _lightsTimerMax = 1;
    private float _lightsTimer;
    private int health = 10;
    public GameObject _explosionPrefab;
    private CameraShake _cameraShake;
    public int _attackRange;
    public int _incomingRange;
    public int _xpReward;
    public GameObject _bullet;
    public int _bulletDamage;
    public void CheckHealth(int _value)
    {
        health -= _value;
        if(health <= 0)
            Die();
    }
    private void Die()
    {
        _gameManager.RemoveEnemy(gameObject);
        //GameObject _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _cameraShake.DoShake(0.001f, 1);
        _player._xp += _xpReward;
        //Destroy(_explosion,5);
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
