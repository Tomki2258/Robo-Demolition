using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 _offset;
    public float _speed;
    public Transform _target;
    void Update()
    {
        var _desiredPosition = _target.position + _offset;
        var _smoothedPosition = Vector3.Lerp(transform.position, _desiredPosition, _speed * Time.deltaTime);
        transform.position = _smoothedPosition;    
    }
}
