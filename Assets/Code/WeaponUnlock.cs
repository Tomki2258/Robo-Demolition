using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUnlock : MonoBehaviour
{
    public List<WeaponClass> _weaponsAtStage;
    public GameObject _weaponUnlockUI;

    [Header("First Element UI")] public TMP_Text _firstWeaponName;

    public Image _firstWeaponImage;

    [Header("Second Element UI")] public TMP_Text _secondWeaponName;

    public Image _secondWeaponImage;
    private Animator _animator;
    private EquipmentCanvas _equipmentCanvas;
    private WeaponClass _firstWeapon;
    private PlayerMovement _playerMovement;
    private PlayerWeapons _playerWeapons;
    private WeaponClass _secondWeapon;
    private UIManager _uiManager;
    private List<int> _weaponUnlockStages;

    private void Start()
    {
        _equipmentCanvas = FindAnyObjectByType<EquipmentCanvas>();
        _playerMovement = FindFirstObjectByType<PlayerMovement>();
        _playerWeapons = _playerMovement._playerWeapons;
        _weaponUnlockStages = _playerMovement._weaponsUnlockStages;
        _animator = GetComponent<Animator>();
        _uiManager = FindAnyObjectByType<UIManager>();
    }

    public void PrepareWeapons()
    {
        if (FindAnyObjectByType<GameManager>()._godMode)
        {
            foreach (var weapon in _weaponsAtStage)
            {
                weapon.UnlockWeapon();
                weapon.SetInUse(false);
            }
            return;
        }
        foreach (var weapon in _weaponsAtStage)
        {
            weapon.SetUnlocked(false);
            weapon.SetInUse(false);
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
        WeaponClass _currentWeapon = null;
        switch (_weaponType.GetWeaponType())
        {
            case WeaponTypes.Shotgun:
                _currentWeapon = FindWeapon(WeaponTypes.Shotgun);
                break;
            case WeaponTypes.MachineGun:
                _currentWeapon = FindWeapon(WeaponTypes.MachineGun);
                break;
            case WeaponTypes.LaserGum:
                _currentWeapon = FindWeapon(WeaponTypes.LaserGum);
                break;
            case WeaponTypes.OrbitalGun:
                _currentWeapon = FindWeapon(WeaponTypes.OrbitalGun);
                break;
            case WeaponTypes.SniperGun:
                _currentWeapon = FindWeapon(WeaponTypes.SniperGun);
                break;
            case WeaponTypes.CircleGun:
                _currentWeapon = FindWeapon(WeaponTypes.CircleGun);
                break;
            case WeaponTypes.SphereAttack:
                _currentWeapon = FindWeapon(WeaponTypes.SphereAttack);
                break;
            case WeaponTypes.RocketLauncher:
                _currentWeapon = FindWeapon(WeaponTypes.RocketLauncher);
                break;
            case WeaponTypes.MineDeployer:
                _currentWeapon = FindWeapon(WeaponTypes.MineDeployer);
                break;
            case WeaponTypes.GranadeLauncher:
                _currentWeapon = FindWeapon(WeaponTypes.GranadeLauncher);
                break;
        }

        if (_currentWeapon != null) _currentWeapon.UnlockWeapon();

        if (_equipmentCanvas.HasFreeWeaponSlot())
        {
            _equipmentCanvas._weaponsInUse.Add(_currentWeapon);
            _currentWeapon.SetInUse(true);
        }

        _weaponUnlockUI.SetActive(false);
        _uiManager.DoLevelUpCanvas(false);

        _playerWeapons.SetWeaponsInUse();

        if (_playerMovement._activeWeaponModels <= _playerMovement._weaponsModels.Count)
        {
            _playerMovement._weaponsModels[_playerMovement._activeWeaponModels].SetActive(true);
            _playerMovement._activeWeaponModels++;   
        }
        
        _uiManager._newEnemySpottedUI.GetComponent<AudioSource>().Play();
    }

    private void SetWeaponsUI()
    {
        var levelOffset = _weaponUnlockStages.IndexOf(_playerMovement._level);
        _firstWeapon = _weaponsAtStage[levelOffset * 2];
        _secondWeapon = _weaponsAtStage[levelOffset * 2 + 1];

        Debug.LogWarning($"First {_firstWeapon} Second {_secondWeapon}");

        _firstWeaponName.text = _firstWeapon.GetWeaponName();
        _firstWeaponImage.sprite = _firstWeapon.GetWeaponSprite();

        _secondWeaponName.text = _secondWeapon.GetWeaponName();
        _secondWeaponImage.sprite = _secondWeapon.GetWeaponSprite();
    }

    private WeaponClass FindWeapon(WeaponTypes _weaponType)
    {
        foreach (var weapon in _weaponsAtStage)
            if (weapon.GetWeaponType() == _weaponType)
                return weapon;
        return null;
    }
}