using System;
using TMPro;
using UnityEngine;

public class StatsCanvas : MonoBehaviour
{
    public GameObject _playerObject;

    [Header("Text")] [SerializeField] private TMP_Text _healthText;

    [SerializeField] private TMP_Text _regenerationText;
    [SerializeField] private TMP_Text _rangeText;
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private TMP_Text _reloadSpeedText;
    private PlayerMovement _player;
    private PlayerWeapons _playerWeapons;
    private float _startDamage;


    private float _startHealth;
    private float _startRange;
    private float _startRegeneration;
    private float _startReloadSpeed;

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
        _startRegeneration =_player._hpRegenMultipler;
        _startDamage = _playerWeapons._damageMultipler;
        _startReloadSpeed = _playerWeapons._reloadMultipler;
    }

    private double PercentageDifference(float start, float end)
    {
        var _result = Math.Abs(start - end) /
                      (start + end) / 2;
        _result *= 100;
        return Math.Round(_result, 1, MidpointRounding.ToEven);
    }

    public void SetStatsCanvas()
    {
        _healthText.text = $"Health bonus {_player._maxHealth - _startHealth}";
        _regenerationText.text = $"Regeneration bonus {_player._maxHealth * _player._hpRegenMultipler}/s";
        _rangeText.text = $"Attack range bonus {_player._attackRange - _startRange}";
        _damageText.text = $"Damage bonus {_playerWeapons._damageMultipler} %";
        //_damageText.text = $"Damage bonus {PercentageDifference(_startDamage, _playerWeapons._standardGunClass.GetDamage())} %";
        _reloadSpeedText.text =
            $"Reload speed bonus {PercentageDifference(_startReloadSpeed,  _playerWeapons._reloadMultipler)} % ";
    }
}