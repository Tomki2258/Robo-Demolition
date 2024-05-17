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
    public int _directionX;
    public bool _testX;
    private int _directionY;
    private bool _testY;
    private bool _goVertical;
    public Vector3 _planesVector;
    public Quaternion _planesQuaterion;
    public void Start()
    {
        _goVertical = Convert.ToBoolean(Random.Range(0,2));
        if (_goVertical)
        {
            _testY = Convert.ToBoolean(Random.Range(0,2));
            Debug.LogWarning(_testY);
            _directionY = _testY ? 1 : -1;
            _planesVector = new Vector3(0, 0, _directionY);
            // _planes.transform.localRotation =  new Quaternion(0, _testY ? 90 : -90, 0, 0);
            _planes.transform.Rotate(0, _testY ? 90 : -90, 0);
        }
        else
        {
            _testX = Convert.ToBoolean(Random.Range(0,2));
            Debug.LogWarning(_testX);
            _directionX = _testX ? 1 : -1;
            _planesVector = new Vector3(_directionX, 0, 0);
            //_planes.transform.localRotation = new Quaternion(0, _testX ? 180 : 0, 0, 0); 
            _planes.transform.Rotate(0, _testX ? 180 : 0, 0);
        }
        //transform.localRotation = _bombsQuaterion;
        int _randomSpawn = Random.Range(0, _bombSpawns.Count);
        _targetSpawn = _bombSpawns[_randomSpawn];
        _planes.transform.position = _targetSpawn.position;
      
        for (int i = 0; i < _bombLenght; i++)
        {
            Vector3 _spawnVector = new Vector3(
                _targetSpawn.transform.position.x + ((20 * i) * _directionX),
                _targetSpawn.transform.position.y,
                _targetSpawn.transform.position.z + ((20 * i) * _directionY));
            StartCoroutine(SpawnExplosion(i * _bombDelay, _spawnVector));
        }
    }

    private void FixedUpdate()
    {
        transform.position += _planesVector * Time.deltaTime * _planesSpeed;
    }

    public IEnumerator SpawnExplosion(float  _delay,Vector3 _vector)
    {
        yield return new WaitForSeconds(_delay);
        Instantiate(_explosionPrefab, _vector, Quaternion.identity);
    }
}
