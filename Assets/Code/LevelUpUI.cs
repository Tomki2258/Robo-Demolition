using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelUpUI : MonoBehaviour
{
    public List<PowerUpClass> _powerUpsList;
    private int _powerUpsCount = 0;
    private PowerUpClass _leftPowerUp;
    private PowerUpClass _rightPowerUp;
    private PlayerMovement _player;
    [Header("Power Up Values")] public float _hpMultipler;
    public float _damageMultipler;
    public float _rangeMultipler;
    public float _regenerationMultipler;
    public float _reloadSpeed;
    private UIManager _uiManager;

    public GameObject _leftPowerUpUI;
    public GameObject _rightPowerUpUI;
    private void Awake()
    {
        _uiManager = FindAnyObjectByType<UIManager>();
        _player = FindAnyObjectByType<PlayerMovement>();
        //_powerUpsList.Add(new PowerUpClass(PowerUpsEnum.Health,$"Increase health by {_hpMultipler} %"));
        _powerUpsList.Add(new PowerUpClass(PowerUpsEnum.Damage,$"Increase damage by {_damageMultipler} %"));
        _powerUpsList.Add(new PowerUpClass(PowerUpsEnum.Range,$"Increase range by {_rangeMultipler} %"));
        _powerUpsList.Add(new PowerUpClass(PowerUpsEnum.Regeneration,$"Increase regeneration by {_regenerationMultipler} %"));
        _powerUpsList.Add(new PowerUpClass(PowerUpsEnum.ReloadSped,$"Increase reload speed by {_reloadSpeed} %"));

        _powerUpsCount = _powerUpsList.Count;

        _damageMultipler *= 0.01f;
        _hpMultipler *= 0.01f;
        _rangeMultipler *= 0.01f;
        _regenerationMultipler *= 0.01f;
        _reloadSpeed *= 0.01f;
    }

    public void ChooseReward(bool _isLeft)
    {
        if (_isLeft)
        {
            ApplyPowerUp(_leftPowerUp);
            return;
        }
        ApplyPowerUp(_rightPowerUp);
    }

    private void ApplyPowerUp(PowerUpClass _currentPowerUp)
    {
        switch (_currentPowerUp.GetPowerUpType())
        {
            case PowerUpsEnum.Health:
                _player._maxHealth *= 1 + _hpMultipler;
                //Debug.LogWarning(PowerUpsEnum.Health);
                break;
            case PowerUpsEnum.Damage:
                _player._playerWeapons._bulletDamage *= 1 + _damageMultipler;
                //Debug.LogWarning(PowerUpsEnum.Damage);
                break;
            case PowerUpsEnum.Range:
                _player._attackRange *= 1 + _rangeMultipler;
                //Debug.LogWarning(PowerUpsEnum.Range);
                break;
            case PowerUpsEnum.Regeneration:
                _player._hpRegenMultipler *= 1 + _regenerationMultipler;
                //Debug.LogWarning(PowerUpsEnum.Regeneration);
                break;
            case PowerUpsEnum.ReloadSped:
                _player._playerWeapons._standardMaxTimer *= 1 - _reloadSpeed;
                _player._playerWeapons._laserMaxTimer *= 1 - _reloadSpeed;
                _player._playerWeapons._rocketMaxTimer *= 1 - _reloadSpeed;
                _player._playerWeapons._circleGunMaxTimer *= 1 - _reloadSpeed;
                _player._playerWeapons._sphereAttackMaxTimer *= 1 - _reloadSpeed;
                _player._playerWeapons._shotgunMaxTimer *= 1 - _reloadSpeed;
                //Debug.LogWarning(PowerUpsEnum.ReloadSped);
                break;
        }
        _uiManager.DoLevelUpCanvas(false);
    }
    public void SetReward()
    {
        int _randomOne = Random.Range(0, _powerUpsCount);
        _leftPowerUp = _powerUpsList[_randomOne];
        
        int _randomTwo = Random.Range(0, _powerUpsCount);
        while (_randomTwo == _randomOne)
        {
            _randomTwo = Random.Range(0, _powerUpsCount);
        }
        _rightPowerUp = _powerUpsList[_randomTwo];
        //Debug.LogWarning($"{_leftPowerUp.GetPowerUpType()} / {_rightPowerUp.GetPowerUpType()}");
        
        //Set UI
        //_leftPowerUpUI.transform.GetChild(1).GetComponent<Image>().sprite =
        _leftPowerUpUI.transform.GetChild(2).GetComponent<TMP_Text>().text = _leftPowerUp.GetPowerUpDescription();
        
        //_rightPowerUpUI.transform.GetChild(1).GetComponent<Image>().sprite =
        _rightPowerUpUI.transform.GetChild(2).GetComponent<TMP_Text>().text = _rightPowerUp.GetPowerUpDescription();
    }
}
