using UnityEngine;

public class PlayerAtributtes : MonoBehaviour
{
    [Header("Bullet Types")] 
    public GameObject _currentBulletType;

    public GameObject _standardBullet;
    public GameObject _explosiveBullet;

    public float _fovBonus;
    [Header("Bullet dodge")] public float _dodgeChange;

    [Header("Bullet Reflection")] 
    public float _bulletReflection;
    [Header("Player Luck")]
    public float _playerLuck;

    public void SwitchBulletType(int _typeIndex)
    {
        switch (_typeIndex)
        {
            case 0:
                _currentBulletType = _standardBullet;
                break;
            case 1:
                _currentBulletType = _explosiveBullet;
                break;
        }
    }

    public bool BulletReflection()
    {
        var _randomValue = Random.Range(0, 100);
        if (_randomValue < _bulletReflection) return true;

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
        var _randomValue = Random.Range(0, 100);
        if (_randomValue < _dodgeChange) return true;

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
    public bool PlayerLuck()
    {
        var _randomValue = Random.Range(0, 100);
        return _randomValue < _playerLuck;
    }
    public void IncreasePlayerLuck()
    {
        if (_playerLuck == 0)
        {
            _playerLuck = 2;
            return;
        }

        _playerLuck *= 1.1f;
    }
}