using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BombardEnemy : MonoBehaviour
{
    public GameObject[] _bombSpawns;
    public GameObject _explosionPrefab;
    public int _bombLenght;
    public float _bombDelay;
    public GameObject _planes;
    public int _planesSpeed;
    public Vector3 _planesVector;
    public Vector3 _planesOffset;
    public GameObject _targetPrefab;
    public List<GameObject> _targetsPrefabs;
    public int _bombardDamage;
    private AudioSource _audioSource;
    private int _directionX;
    private int _directionY;
    private bool _goVertical;
    private Transform _targetSpawn;
    private bool _testX;
    private bool _testY;

    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, 12);
        _bombSpawns = GameObject.FindGameObjectsWithTag("bombersSpawner");
        _goVertical = Convert.ToBoolean(Random.Range(0, 2));
        var _randomSpawn = Random.Range(0, _bombSpawns.Length);
        _targetSpawn = _bombSpawns[_randomSpawn].transform;

        if (_targetSpawn.GetComponent<Intersection>()._inUse) return;
        var _intersection = _targetSpawn.GetComponent<Intersection>();
        var _possibleVectors = _intersection._possibleVectors;

        if (_goVertical)
        {
            if (_possibleVectors.y == 0)
            {
                _testY = Convert.ToBoolean(Random.Range(0, 2));
                Debug.LogWarning(_testY);
                _directionY = _testY ? 1 : -1;
                _planesVector = new Vector3(0, 0, _directionY);
                _planes.transform.Rotate(0, _testY ? 90 : -90, 0);
                _planesOffset = new Vector3(
                    0
                    , _planesOffset.y
                    , -_planesOffset.z * _directionY);
            }
            else if (_possibleVectors.y == 1)
            {
                _directionY = 1;
                _planesVector = new Vector3(0, 0, _directionY);
                _planes.transform.Rotate(0, 90, 0);
                _planesOffset = new Vector3(
                    0
                    , _planesOffset.y
                    , -_planesOffset.z * _directionY);
            }
            else
            {
                _directionY = -1;
                _planesVector = new Vector3(0, 0, _directionY);
                _planes.transform.Rotate(0, -90, 0);
                _planesOffset = new Vector3(
                    0
                    , _planesOffset.y
                    , -_planesOffset.z * _directionY);
            }
        }
        else
        {
            if (_possibleVectors.x == 0)
            {
                _testX = Convert.ToBoolean(Random.Range(0, 2));
                Debug.LogWarning(_testX);
                _directionX = _testX ? 1 : -1;
                _planesVector = new Vector3(_directionX, 0, 0);
                _planes.transform.Rotate(0, _testX ? 180 : 0, 0);
                _planesOffset = new Vector3(
                    -_planesOffset.x * _directionX
                    , _planesOffset.y
                    , 0);
            }
            else if (_possibleVectors.x == 1)
            {
                _directionX = 1;
                _planesVector = new Vector3(_directionX, 0, 0);
                _planes.transform.Rotate(0, 180, 0);
                _planesOffset = new Vector3(
                    -_planesOffset.x * _directionX
                    , _planesOffset.y
                    , 0);
            }
            else
            {
                _directionX = -1;
                _planesVector = new Vector3(_directionX, 0, 0);
                _planes.transform.Rotate(0, 0, 0);
                _planesOffset = new Vector3(
                    -_planesOffset.x * _directionX
                    , _planesOffset.y
                    , 0);
            }
        }

        _planes.transform.position = _targetSpawn.position + _planesOffset;
        for (var i = 0; i < _bombLenght; i++)
        {
            var _spawnVector = new Vector3(
                _targetSpawn.transform.position.x + 20 * i * _directionX,
                _targetSpawn.transform.position.y + 0.5f,
                _targetSpawn.transform.position.z + 20 * i * _directionY + 6);
            var _target = Instantiate(_targetPrefab, _spawnVector, quaternion.identity);
            _target.transform.localScale *= 5;
            _targetsPrefabs.Add(_target);
        }

        _targetSpawn.GetComponent<Intersection>()._inUse = true;
        StartCoroutine(DoAirstrike());
    }

    private void FixedUpdate()
    {
        if (_planes.activeSelf)
            transform.position += _planesVector * Time.deltaTime * _planesSpeed;
    }

    public IEnumerator DoAirstrike()
    {
        _planes.SetActive(false);
        yield return new WaitForSeconds(3);
        _audioSource.Play();
        yield return new WaitForSeconds(3);
        _planes.SetActive(true);
        _targetSpawn.GetComponent<Intersection>()._inUse = false;
        for (var i = 0; i < _bombLenght; i++)
        {
            var _spawnVector = new Vector3(
                _targetSpawn.transform.position.x + 20 * i * _directionX,
                _targetSpawn.transform.position.y,
                _targetSpawn.transform.position.z + 20 * i * _directionY + 2);
            StartCoroutine(SpawnExplosion(i, _spawnVector));
        }
    }

    public IEnumerator SpawnExplosion(float _delay, Vector3 _vector)
    {
        yield return new WaitForSeconds(_delay * _bombDelay);
        Destroy(_targetsPrefabs[Convert.ToInt16(_delay)]);
        var _explpsionInstance = Instantiate(_explosionPrefab, _vector, Quaternion.identity);
        Destroy(_explpsionInstance, 5);
        var _colliders = Physics.OverlapSphere(_vector, 10);
        foreach (var _obj in _colliders)
            if (_obj.CompareTag("Player"))
            {
                var _player = _obj.GetComponent<PlayerMovement>();
                _player.CheckHealth(_bombardDamage);
                break;
            }
    }
}