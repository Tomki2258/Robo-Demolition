using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 _offset;
    public float _speed;
    public Transform _target;
    public float _rotationSpeed;
    private GameManager _gameManager;
    public GameObject _startCapsule;
    private Vector3 _oldOffset;
    private void Start()
    {
        _oldOffset = _offset;
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
    }

    private void Update()
    {
        if(_gameManager._gameStarted) DoCamera();
    }

    private void DoCamera()
    {
        var _desiredPosition = _target.position + _offset * _target.localScale.x;
        var _smoothedPosition = Vector3.Lerp(transform.position, _desiredPosition, _speed * Time.deltaTime);
        transform.position = _smoothedPosition;

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

    public void SetOldOffset()
    {
        _offset = _oldOffset;
    }
}