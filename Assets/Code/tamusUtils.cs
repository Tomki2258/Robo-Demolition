using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class tamusUtils
{
    public static float GetDistance(GameObject _one, GameObject _two)
    {
        return Vector3.Distance(_one.transform.position, _two.transform.position);
    }

    public static void MoveTurret(Vector3 _target,Transform _transform,Transform _top,float _turretRotateSpeed)
    {
        var _direction = _target - _transform.position;
        _direction.Normalize();
        _top.rotation = Quaternion.Slerp(_top.rotation, Quaternion.LookRotation(_direction),
            _turretRotateSpeed * Time.deltaTime);
    }
}
