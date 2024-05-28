using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using NUnit.Framework;
using UnityEngine;

public class WeaponUnlock : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private List<int> _weaponUnlockStages;
    private Animator _animator;
    public List<WeaponTypes> _weaponsAtStage;
    private WeaponTypes _firstWeapon;
    private WeaponTypes _secondWeapon;
    void Start()
    {
        _playerMovement = FindFirstObjectByType<PlayerMovement>();
        _weaponUnlockStages = _playerMovement._weaponsUnlockStages;
        _animator = GetComponent<Animator>();
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

    private void GetReward(WeaponTypes _weaponType)
    {
        
    }
    private void SetWeaponsUI()
    {
        int levelOffset = _weaponUnlockStages.IndexOf(_playerMovement._level);
        _firstWeapon = _weaponsAtStage[levelOffset * 2];
        _secondWeapon = _weaponsAtStage[levelOffset * 2 + 1];
        
        Debug.LogWarning($"First {_firstWeapon} Second {_secondWeapon}");
    }
    public enum WeaponTypes
    {
        Standard,
        MachineGun,
        Shotgun,
        LaserGum,
        OrbitalGun,
        SniperGun,
        CircleGun,
        SphereAttack
    }
}
