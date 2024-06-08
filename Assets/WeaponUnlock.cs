using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class WeaponUnlock : MonoBehaviour
{
    private UIManager _uiManager;
    private PlayerMovement _playerMovement;
    private List<int> _weaponUnlockStages;
    private Animator _animator;
    private PlayerWeapons _playerWeapons;
    public List<WeaponClass> _weaponsAtStage;
    private WeaponClass _firstWeapon;
    private WeaponClass _secondWeapon;
    public GameObject _weaponUnlockUI;
    [Header("First Element UI")]
    public TMP_Text _firstWeaponName;
    public Image _firstWeaponImage;
    
    [Header("Second Element UI")]
    public TMP_Text _secondWeaponName;
    public Image _secondWeaponImage;
    
    void Start()
    {
        _playerMovement = FindFirstObjectByType<PlayerMovement>();
        _playerWeapons = _playerMovement._playerWeapons;
        _weaponUnlockStages = _playerMovement._weaponsUnlockStages;
        _animator = GetComponent<Animator>();
        _uiManager = FindAnyObjectByType<UIManager>();
    }
    public void PrepareWeapons(){
        foreach (WeaponClass weapon in _weaponsAtStage)
        {
            weapon.SetUnlocked(false);
        }
    }

    public bool CheckForWeaponUnlock()
    {
        if (_weaponUnlockStages.Contains(_playerMovement._level))
        {
            Debug.LogWarning("Weapon unlock");
            _animator.SetTrigger("ChooseWeapon");
            SetWeaponsUI();
            return true;
        }

        return false;
    }

    public void ChooseReward(bool _isLeft)
    {
        if (_isLeft)
        {
            GetReward(_firstWeapon);
            return;
        }
        GetReward(_secondWeapon);
    }

    private void GetReward(WeaponClass _weaponType)
    {
        _weaponType.UnlockWeapon();
        _playerWeapons._weaponsInUse.Add(_weaponType);
        switch (_weaponType.GetWeaponType())
        {
            case WeaponTypes.Shotgun:
                _playerWeapons._shotgunEnabled = true;
                break;
            case WeaponTypes.MachineGun:
                _playerWeapons._machineGunEnabled = true;
                break;
            case WeaponTypes.LaserGum:
                _playerWeapons._laserGunEnabled = true;
                break;
            case WeaponTypes.OrbitalGun:
                _playerWeapons._rocketLauncherEnabled = true;
                break;
            case WeaponTypes.SniperGun:
                _playerWeapons._sniperGunEnabled = true;
                break;
            case WeaponTypes.CircleGun:
                _playerWeapons._circleGunEnabled = true;
                break;
            case WeaponTypes.SphereAttack:
                _playerWeapons._sphereAttackEnabled = true;
                break;
            case WeaponTypes.RocketLauncher:
                _playerWeapons._rocketLauncherEnabled = true;
                break;
        }
        _weaponUnlockUI.SetActive(false);
        _uiManager.DoLevelUpCanvas(false);
    }
    private void SetWeaponsUI()
    {
        int levelOffset = _weaponUnlockStages.IndexOf(_playerMovement._level);
        _firstWeapon = _weaponsAtStage[levelOffset * 2];
        _secondWeapon = _weaponsAtStage[levelOffset * 2 + 1];
        
        Debug.LogWarning($"First {_firstWeapon} Second {_secondWeapon}");

        _firstWeaponName.text = _firstWeapon.GetWeaponName();
        _firstWeaponImage.sprite = _firstWeapon.GetWeaponSprite();
        
        _secondWeaponName.text = _secondWeapon.GetWeaponName();
        _secondWeaponImage.sprite = _secondWeapon.GetWeaponSprite();
    }
}
