using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentCanvas : MonoBehaviour
{
    [SerializeField] private List<WeaponPanel> _weaponPanels;
    public List<WeaponClass> _weaponsInUse;
    [SerializeField] private PlayerWeapons _playerWeapons;
    public int _maxWeaponsInUse;
    [SerializeField] private TMP_Text _weaponAmountText;
    [SerializeField] private ScrollRect _scrollRect;
    private void Start()
    {
        _playerWeapons._weaponsInUse[0].SetInUse(true);
        CheckForWeaponPanels();
    }
    
    public void CheckForWeaponPanels()
    {
        SortWeapons();
        Debug.LogWarning("Weapons panels checks");
        foreach (var _weaponPanel in _weaponPanels) _weaponPanel.CheckForUnlock();

        _weaponsInUse = _playerWeapons.GetWeapons();
        _weaponsInUse.RemoveAll(s => s == null);
        RefleshWeaponAmountInfo();
    }

    public void RefleshWeaponAmountInfo()
    {
        _weaponAmountText.text = $"{_weaponsInUse.Count}/{_maxWeaponsInUse}";
    }

    public bool HasFreeWeaponSlot()
    {
        return _weaponsInUse.Count < _maxWeaponsInUse;
    }
    public int GetUsedWeapons()
    {
        return _maxWeaponsInUse + 1 - _weaponsInUse.Count;
    }

    private void SortWeapons()
    {
        _weaponPanels.Sort((weaponA, weaponB) => weaponB._weaponClass.IsWeaponUnlocked().CompareTo(weaponA._weaponClass.IsWeaponUnlocked()));

        for (int i = 0; i < _weaponPanels.Count; i++)
        {
            _weaponPanels[i].transform.SetSiblingIndex(i);
        }
    }
}