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
    private UIManager _uiManager;

    public GameObject _leftPowerUpUI;
    public GameObject _rightPowerUpUI;
    public StatsCanvas _statsCanvas;

    private WeaponUnlock _weaponUnlock;
    private void Awake()
    {
        _weaponUnlock = GetComponent<WeaponUnlock>();
        _weaponUnlock.gameObject.SetActive(false);
        _uiManager = FindAnyObjectByType<UIManager>();
        _player = FindAnyObjectByType<PlayerMovement>();
        //_powerUpsList.Add(new PowerUpClass(PowerUpsEnum.Health,$"Increase health by {_hpMultipler} %"));
        /*
        _powerUpsList.Add(new PowerUpClass(PowerUpsEnum.Damage,$"Increase damage by {_damageMultipler} %"));
        _powerUpsList.Add(new PowerUpClass(PowerUpsEnum.Range,$"Increase range by {_rangeMultipler} %"));
        _powerUpsList.Add(new PowerUpClass(PowerUpsEnum.Regeneration,$"Increase regeneration by {_regenerationMultipler} %"));
        _powerUpsList.Add(new PowerUpClass(PowerUpsEnum.ReloadSped,$"Increase reload speed by {_reloadSpeed} %"));_reloadSpeed
        */
        _powerUpsCount = _powerUpsList.Count;
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
                _player._maxHealth *= 1 + _currentPowerUp.GetPlayerBonus();
                //Debug.LogWarning(PowerUpsEnum.Health);
                break;
            case PowerUpsEnum.Damage:
                _player._playerWeapons.ModyfyDamage(_currentPowerUp.GetPlayerBonus());
                //Debug.LogWarning(PowerUpsEnum.Damage);
                break;
            case PowerUpsEnum.Range:
                _player._attackRange *= 1 + _currentPowerUp.GetPlayerBonus();
                //Debug.LogWarning(PowerUpsEnum.Range);
                break;
            case PowerUpsEnum.Regeneration:
                _player._hpRegenMultipler *= 1 + _currentPowerUp.GetPlayerBonus();
                //Debug.LogWarning(PowerUpsEnum.Regeneration);
                break;
            case PowerUpsEnum.ReloadSped:
                _player._playerWeapons.ModyfyReloadSpeed(_currentPowerUp.GetPlayerBonus());
                //Debug.LogWarning(PowerUpsEnum.ReloadSped);
                break;
        }

        if (_weaponUnlock.CheckForWeaponUnlock())
        {
            _weaponUnlock._weaponUnlockUI.SetActive(true);
        }
        else
        {
            _uiManager.DoLevelUpCanvas(false);
            _weaponUnlock._weaponUnlockUI.SetActive(false);
        }
    }
    public void SetReward()
    {
        _statsCanvas.SetStatsCanvas();
        
        int _randomOne = Random.Range(0, _powerUpsCount);
        _leftPowerUp = _powerUpsList[_randomOne];
        
        int _randomTwo = Random.Range(0, _powerUpsCount);
        while (_randomTwo == _randomOne)
        {
            _randomTwo = Random.Range(0, _powerUpsCount);
        }
        _rightPowerUp = _powerUpsList[_randomTwo];
        
        //Set UI
        _leftPowerUpUI.transform.GetChild(1).GetComponent<Image>().sprite = _leftPowerUp.GetPowerUpSprite();
        _leftPowerUpUI.transform.GetChild(2).GetComponent<TMP_Text>().text = _leftPowerUp.GetPowerUpDescription();

        _rightPowerUpUI.transform.GetChild(1).GetComponent<Image>().sprite = _rightPowerUp.GetPowerUpSprite();
        _rightPowerUpUI.transform.GetChild(2).GetComponent<TMP_Text>().text = _rightPowerUp.GetPowerUpDescription();
    }
}
