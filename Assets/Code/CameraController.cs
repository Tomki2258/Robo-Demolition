using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    public Vector3 _offset;
    public float _speed;
    public Transform _target;
    public float _rotationSpeed;
    private GameManager _gameManager;
    public GameObject _startCapsule;
    private List<Vector3> _idleCameraVectors;
    private int _currentIdleIndex = 0;
    private void Start()
    {
        _gameManager = FindFirstObjectByType<GameManager>();
        
        if (_gameManager._gameLaunched)
        {
            //_target = FindFirstObjectByType<PlayerMovement>().transform;
        }
        else
        {
            _target = _startCapsule.transform;
            _offset.x = 5;
        }
        
        _idleCameraVectors.Add(new Vector3(transform.position.x - 2,
            transform.position.y + 1,
            transform.position.z + 3));
        _idleCameraVectors.Add(new Vector3(transform.position.x + 2,
            transform.position.y - 1,
            transform.position.z - 3));
        _idleCameraVectors.Add(new Vector3(transform.position.x + 2,
            transform.position.y - 1,
            transform.position.z - 3));
        _idleCameraVectors.Add(new Vector3(transform.position.x + 2,
            transform.position.y - 1,
            transform.position.z - 3));
    }

    private void Update()
    {
        if(_gameManager._gameStarted)
        {
            DoCamera();
            _speed = 3;
        }
        else
        {
            DoIdleCamera();
            _speed = 1.5f;
        }
    }

    private void DoIdleCamera()
    {
        if (Vector3.Distance(transform.position, _idleCameraVectors[_currentIdleIndex]) < 0.05f)
        {
            _currentIdleIndex = Random.Range(0, _idleCameraVectors.Count);
        }
        MoveToPosition(_idleCameraVectors[_currentIdleIndex]);
    }

    private void MoveToPosition(Vector3 targetPostion)
    {
        var _smoothedPosition = Vector3.Lerp(transform.position, targetPostion, _speed * Time.deltaTime);
        transform.position = _smoothedPosition;
    }
    private void DoCamera()
    {
        var _desiredPosition = _target.position + _offset * _target.localScale.x;
        
        MoveToPosition(_desiredPosition);

        var lookDirection = _target.position - transform.position;
        lookDirection.Normalize();

        transform.rotation
            = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection),
                _rotationSpeed * Time.deltaTime);
    }

    public void SwitchTarget(Transform _target)
    {
        this._target = _target;

        if (this._target.CompareTag("Player"))
        {
            _offset.x = 0;
        }
    }
}