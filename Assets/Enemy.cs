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
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        _agent.SetDestination(_player.transform.position);
    }

    private void Die()
    {
        _gameManager.RemoveEnemy(gameObject);
    }
}
