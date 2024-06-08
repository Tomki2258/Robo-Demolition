using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using NUnit.Framework;
using UnityEngine;

public class EquipmentCanvas : MonoBehaviour
{
    public List<WeaponPanel> _weaponPanels;
    private List<WeaponClass> _weaponsInUse;
    private PlayerWeapons _playerWeapons;
    public List<WeaponPanel> _weaponPlaces;
    private void Start()
    {
        _playerWeapons = FindFirstObjectByType<PlayerWeapons>();
    }

    public void CheckForWeaponPanels()
    {
        _weaponsInUse = _playerWeapons._weaponsInUse;

        foreach (WeaponClass _weaponClass in _weaponsInUse)
        {
            _weaponPlaces[_weaponsInUse.IndexOf(_weaponClass)]._weaponClass = _weaponClass;
        }
        
        foreach (WeaponPanel weaponPanel in _weaponPanels)
        {
            weaponPanel.CheckForUnlock();
        }
    }
}
