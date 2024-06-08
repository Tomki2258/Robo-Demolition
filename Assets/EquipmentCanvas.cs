using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EquipmentCanvas : MonoBehaviour
{
    public List<WeaponPanel> _weaponPanels;

    private void OnEnable()
    {
        CheckForWeaponPanels();
    }

    private void CheckForWeaponPanels()
    {
        foreach (WeaponPanel weaponPanel in _weaponPanels)
        {
            weaponPanel.CheckForUnlock();
        }
    }
}
