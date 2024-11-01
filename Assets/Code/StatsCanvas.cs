using System;
using TMPro;
using UnityEngine;

public class StatsCanvas : MonoBehaviour
{
    public GameObject _playerObject;
    [SerializeField] private CameraController _cameraController;
    [Header("Text")]
    [SerializeField] private TMP_Text _regenerationText;
    [SerializeField] private TMP_Text _rangeText;
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private TMP_Text _reloadSpeedText;
    [SerializeField] private TMP_Text _bulletDodgeText;
    [SerializeField] private TMP_Text _fovText;
    [SerializeField] private TMP_Text _bonusHPText;
    [SerializeField] private TMP_Text _luckyText;
    private PlayerMovement _player;
    private PlayerWeapons _playerWeapons;
    private PlayerAtributtes _playerAtributtes;
    private float _startDamage;


    private float _startHealth;
    public float _bonusHP = 0;
    private float _startRange;
    private float _startRegeneration;
    private float _startReloadSpeed;
    private float _startBulletDodge;
    private float _startFov;
    private float _lucky;
    private void Start()
    {
        _player = _playerObject.GetComponent<PlayerMovement>();
        _playerWeapons = _playerObject.GetComponent<PlayerWeapons>();
        _playerAtributtes = _player.GetComponent<PlayerAtributtes>();
        SetValues();
    }

    private void SetValues()
    {
        _startHealth = _player._maxHealth;
        _startRange = _player._attackRange;
        _startRegeneration =_player._hpRegenMultipler - 1;
        _startDamage = _playerWeapons._damageMultipler;
        _startReloadSpeed = _playerWeapons._reloadMultipler;
        _startBulletDodge = _playerAtributtes._dodgeChange;
        _startFov = _cameraController._offset.y;
        _lucky = _playerAtributtes._playerLuck;
    }

    private double PercentageDifference(float start, float end)
    {
        var _result = Math.Abs(start - end) /
                      (start + end) / 2;
        _result *= 100;
        return Math.Round(_result, 1, MidpointRounding.ToEven);
    }

    private double RoundedValues(double _value)
    {
        return Math.Round(_value, 2);
    }
    public void SetStatsCanvas()
    {
        //_healthText.text = $"Health bonus {_player._maxHealth - _startHealth}";
        _regenerationText.text = $"Regeneration {RoundedValues(_player._maxHealth * _player._hpRegenMultipler)}/s";
        _rangeText.text = $"Attack range {RoundedValues(PercentageDifference(_startRange, _player._attackRange))} %";
        _damageText.text = $"Damage {RoundedValues(_playerWeapons._damageMultipler - 1)} %";
        //_damageText.text = $"Damage bonus {PercentageDifference(_startDamage, _playerWeapons._standardGunClass.GetDamage())} %";
        _reloadSpeedText.text =
            $"Reload speed {RoundedValues(PercentageDifference(_startReloadSpeed,  _playerWeapons._reloadMultipler))} % ";
        _bulletDodgeText.text = $"Bullet dodge {RoundedValues(_player._playerAtributtes._dodgeChange)} %";
        _fovText.text = $"Field of view {RoundedValues(_player._playerAtributtes._fovBonus * 100)} %";
        _bonusHPText.text = $"Bonus HP {_bonusHP}";
        _luckyText.text = $"Luck {_playerAtributtes._playerLuck} %";
    }
}