using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtributtes : MonoBehaviour
{
    [Header("Bullet dodge")] 
    public float _dodgeChange;

    [Header("Bullet Reflection")] public float _bulletReflection;
    public bool BulletReflection()
    {
        int _randomValue = Random.Range(0, 100);
        if (_randomValue < _dodgeChange)
        {
            return true;
        }

        return false;
    }

    public void IncreaceBulletReflection()
    {
        if (_bulletReflection == 0)
        {
            _bulletReflection = 5;
            return;
        }

        _bulletReflection *= 1.1f;
    }
    public bool BulletDodge()
    {
        int _randomValue = Random.Range(0, 100);
        if (_randomValue < _dodgeChange)
        {
            return true;
        }

        return false;
    }

    public void IncreaseDodgeRate()
    {
        if (_dodgeChange == 0)
        {
            _dodgeChange = 5;
            return;
        }

        _dodgeChange *= 1.1f;
    }
}
