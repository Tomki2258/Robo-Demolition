using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StatsCanvas : MonoBehaviour
{
   public GameObject _playerObject;
    private PlayerMovement _player;
    private PlayerWeapons _playerWeapons;
    
    
    private float _startHealth;
    private float _startRegeneration;
    private float _startReloadSpeed;
    private float _startRange;
    private float _startDamage;
    [Header("Text")] 
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _regenerationText;
    [SerializeField] private TMP_Text _rangeText;
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private TMP_Text _reloadSpeedText;

    private void Start()
    {
        SetValues();
    }

    public void SetValues()
    {
        _player = _playerObject.GetComponent<PlayerMovement>();
        _playerWeapons = _playerObject.GetComponent<PlayerWeapons>();

        _startHealth = _player._maxHealth;
        _startRange = _player._attackRange;
        _startRegeneration = _playerWeapons._standardMaxTimer;
        _startDamage = _playerWeapons._bulletDamage;
        _startReloadSpeed = _playerWeapons._standardMaxTimer;
    }

    private double PercentageDifference(float start, float end)
    {
        float _result = Math.Abs(start - end) /
            (start + end) / 2;
        _result *= 100;
        return Math.Round(_result ,1, MidpointRounding.ToEven);
    }
    public void SetStatsCanvas()
    {
        _healthText.text = $"Health bonus {_player._maxHealth - _startHealth}";
        _regenerationText.text = $"Regeneration bonus {_player._maxHealth * _player._hpRegenMultipler}/s";
        _rangeText.text = $"Attack range bonus {_player._attackRange - _startRange}";
        _damageText.text = $"Damage bonus {PercentageDifference(_startDamage,_playerWeapons._bulletDamage)} %";
        _reloadSpeedText.text = $"Reload speed bonus {PercentageDifference(_startReloadSpeed,_playerWeapons._standardMaxTimer)} % ";
    }
}
