using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _placableObjects;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
    }

    public void AddTurrte(GameObject turret)
    {
        _placableObjects.Add(turret);
    }
    public void CheckTurrets()
    {
        int constructorsCount = 0;

        for (int i = 0; i < _gameManager._spawnedEnemies.Count; i++)
        {
            if (_gameManager._spawnedEnemies[i] != null &&
                _gameManager._spawnedEnemies[i].GetComponent<ConstructorEnemy>())
            {
                constructorsCount++;
            }
        }
        
        if (_placableObjects.Count > constructorsCount * 3)
        {
            Debug.LogWarning("OUT OF TURRETS");
            Destroy(_placableObjects[0].gameObject);
            _placableObjects.RemoveAt(0);
        }
    }
}
