using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PowerUpType
{
    Health,
    Shield
}

public class PowerUP : MonoBehaviour
{
    public PowerUpType _powerUpType;
    private UIManager _uiManager;
    private QuestsMonitor _questsMonitor;
    private Transform _meshesParent;
    private void Start()
    {
        _questsMonitor = FindFirstObjectByType<QuestsMonitor>();
        _meshesParent = transform.GetChild(0);
        for (int i = 0; i < _meshesParent.childCount; i++)
        {
            _meshesParent.GetChild(i).gameObject.SetActive(false);
        }
        _uiManager = FindFirstObjectByType<UIManager>();

        var _randomType = Random.Range(0, Enum.GetNames(typeof(PowerUpType)).Length);
        switch (_randomType)
        {
            case 0:
                _powerUpType = PowerUpType.Health;
                _meshesParent.GetChild(0).gameObject.SetActive(true);
                break;
            case 1:
                _powerUpType = PowerUpType.Shield;
                _meshesParent.GetChild(1).gameObject.SetActive(true);
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
                case PowerUpType.Health:
                    if (_player._health >= _player._maxHealth) return;
                    _player._health += Convert.ToInt32(_player._health * 0.33f);
                    _uiManager.ShowHpDifference(_player._health * 0.33f);

                    if (_player._health > _player._maxHealth)
                        _player._health = _player._maxHealth;
                    break;
                case PowerUpType.Shield:
                    _player._shield = true;
                    break;
            }

            _questsMonitor._colledtedPowerUps++;
            Destroy(gameObject);
        }
    }
}