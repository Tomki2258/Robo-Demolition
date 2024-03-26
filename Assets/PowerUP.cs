using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PowerUpType
{
    Speed,
    Health,
    Damage,
    Shield
}

public class PowerUP : MonoBehaviour
{
    public PowerUpType _powerUpType;

    private void Start()
    {
        var _randomType = Random.Range(0, PowerUpType.GetNames(typeof(PowerUpType)).Length);
        switch (_randomType)
        {
            case 0:
                _powerUpType = PowerUpType.Speed;
                break;
            case 1:
                _powerUpType = PowerUpType.Health;
                break;
            // case 2:
            //     _powerUpType = PowerUpType.Damage;
            //     break;
            case 2:
                _powerUpType = PowerUpType.Shield;
                break;
            default:
                break;
        }

        
        //Tutaj będzie ustawianie textur etc na starcie
        switch (_powerUpType)
        {
            case PowerUpType.Speed:
                break;
            case PowerUpType.Health:
                break;
            case PowerUpType.Damage:
                break;
            case PowerUpType.Shield:
                Debug.LogWarning("Shield done");
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            var _player = other.GetComponent<PlayerMovement>();
            switch (_powerUpType)
            {
                case PowerUpType.Speed:
                    _player._speed *= 1.33f;
                    break;
                case PowerUpType.Health:
                    _player._health += Convert.ToInt32(_player._health * 0.33f);
                    _player._maxHealth += Convert.ToInt32(_player._maxHealth * 0.33f);
                    
                    if(_player._health > _player._maxHealth)
                        _player._health = _player._maxHealth;
                    break;
                case PowerUpType.Damage:
                    _player._damage += Convert.ToInt16(_player._damage * 0.33f);
                    break;
                case PowerUpType.Shield:
                    _player._shield = true;
                    break;
            }

            Destroy(gameObject);
        }
    }
}