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
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        _agent.SetDestination(_player.transform.position);
        SwitchLight();
    }

    public void CheckHealth()
    {
        Die();
    }
    private void Die()
    {
        Debug.Log("Enemy died");
        _gameManager.RemoveEnemy(gameObject);
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
