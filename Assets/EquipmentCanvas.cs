using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using NUnit.Framework;
using UnityEngine;

public class EquipmentCanvas : MonoBehaviour
{
    public List<WeaponPanel> _weaponPanels;
    public List<WeaponClass> _weaponsInUse;
    public PlayerWeapons _playerWeapons;
    public List<WeaponPanel> _weaponPlaces;
    public List<WeaponPanel> _clickedButtons = new List<WeaponPanel>(2);

    public void CheckForWeaponPanels()
    {
        foreach (WeaponPanel _weaponPanel in _weaponPanels)
        {
            _weaponPanel.CheckForUnlock();
        }
        
        _weaponsInUse = _playerWeapons.GetWeapons();

        foreach (WeaponClass _weaponClass in _weaponsInUse)
        {
            _weaponPlaces[_weaponsInUse.IndexOf(_weaponClass)]._weaponClass = _weaponClass;
        }
    }

    public void SetUsedWeapons()
    {
        List<WeaponClass> _weaponClasses = new List<WeaponClass>();
        Debug.LogWarning("Weapons set");
        foreach (WeaponPanel _weaponPanel in _weaponPlaces)
        {
            _weaponClasses.Add(_weaponPanel._weaponClass);
        }
        _playerWeapons.SetUsedWeapons(_weaponClasses);
    }

    public int GetFreeWeaponSlot()
    {
        foreach (WeaponPanel _weaponPanel in _weaponPlaces)
        {
            if (_weaponPanel._weaponClass == null)
            {
                return _weaponPlaces.IndexOf(_weaponPanel);
            }
        }

        return -1;
    }
}
