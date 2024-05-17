using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BombardEnemy : MonoBehaviour
{
    public List<Transform> _bombSpawns;
    private Transform _targetSpawn;
    public GameObject _explosionPrefab;
    public int _bombLenght;
    public float _bombDelay;
    public GameObject _planes;
    public int _planesSpeed;
    public Vector3 _bombsDirection;
    public void Start()
    {
        int _randomSpawn = Random.Range(0, _bombSpawns.Count);
        _targetSpawn = _bombSpawns[_randomSpawn];
        for (int i = 0; i < _bombLenght; i++)
        {
            Vector3 _spawnVector = new Vector3(
                _targetSpawn.transform.position.x - (20 * i),
                _targetSpawn.transform.position.y,
                _targetSpawn.transform.position.z);
            StartCoroutine(SpawnExplosion(i * _bombDelay, _spawnVector));
        }
    }

    private void FixedUpdate()
    {
        transform.position += _bombsDirection * Time.deltaTime * _planesSpeed;
    }

    public IEnumerator SpawnExplosion(float  _delay,Vector3 _vector)
    {
        yield return new WaitForSeconds(_delay);
        Instantiate(_explosionPrefab, _vector, Quaternion.identity);
    }
}
