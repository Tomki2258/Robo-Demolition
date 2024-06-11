using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class EquipmentCanvas : MonoBehaviour
{
    public List<WeaponPanel> _weaponPanels;
    public List<WeaponClass> _weaponsInUse;
    public PlayerWeapons _playerWeapons;
    public int _maxWeaponsInUse;
    public TMP_Text _weaponAmountText;
    private void Start()
    {
        _playerWeapons._weaponsInUse[0].SetInUse(true);
    }

    public void CheckForWeaponPanels()
    {
        foreach (WeaponPanel _weaponPanel in _weaponPanels)
        {
            _weaponPanel.CheckForUnlock();
        }
        
        _weaponsInUse = _playerWeapons.GetWeapons();
        _weaponsInUse.RemoveAll(s => s == null);
        RefleshWeaponAmountInfo();
    }

    public void RefleshWeaponAmountInfo()
    {
        _weaponAmountText.text = $"{_weaponsInUse.Count}/{_maxWeaponsInUse}";
    }
}
