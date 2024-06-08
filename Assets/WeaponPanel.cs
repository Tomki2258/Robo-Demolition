using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour
{
    public WeaponClass _weaponClass;
    [SerializeField] private Image _weaponImage;
    public Sprite _weaponSprite;
    public Sprite _lockedSprite;
    public bool _isUnlocked;
    private EquipmentCanvas _equipment;
    void Start()
    {
        if (_weaponClass == null)
        {
            return;
        }
        _equipment = FindFirstObjectByType<EquipmentCanvas>();
        UpdateVisuals();
        CheckForUnlock();
    }

    private void UpdateVisuals()
    {
        if (_weaponClass == null)
        {
            return;
        }
        _weaponSprite = _weaponClass.GetWeaponSprite();
        
        if (_weaponClass.IsWeaponUnlocked())
        {
            _weaponImage.sprite = _weaponSprite;
        }
        else
        {
            _weaponImage.sprite = _lockedSprite;
        }
    }
    public void CheckForUnlock()
    {
        if (_weaponClass == null)
        {
            return;
        }
        if (_weaponClass.IsWeaponUnlocked())
        {
            _weaponImage.sprite = _weaponSprite;
        }
        else
        {
            _weaponImage.sprite = _lockedSprite;
        }
    }
    public void ChooseWeapon()
    {
        if (_equipment._clickedButtons.Count == 0)
        {
            if(_equipment._weaponPlaces.Contains(this))
            {
                _equipment._clickedButtons.Add(this);
            }
        }
        else
        {
            if(_equipment._weaponPlaces.Contains(this))
            {
                Debug.LogWarning("wrong pick !");
                return;
            }
            
            _equipment._clickedButtons.Add(this);
            
            WeaponClass _tempClass = _equipment._clickedButtons[0]._weaponClass;
            _equipment._clickedButtons[0]._weaponClass = _equipment._clickedButtons[1]._weaponClass;
            _equipment._clickedButtons[1]._weaponClass = _tempClass;
            _equipment._clickedButtons[0].UpdateVisuals();
            _equipment._clickedButtons[1].UpdateVisuals();
            _equipment._clickedButtons.Clear();
        }
    }
}
