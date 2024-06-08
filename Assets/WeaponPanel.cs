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
    public EquipmentCanvas _equipment;
    private Button _button;
    void Start()
    {
        _button = transform.GetChild(1).GetComponent<Button>();
        if (_weaponClass == null)
        {
            return;
        }
        UpdateVisuals();
        CheckForUnlock();
    }

    public void UpdateVisuals()
    {
        if (_weaponClass == null)
        {
            return;
        }
        _weaponSprite = _weaponClass.GetWeaponSprite();
        
        CheckForUnlock();
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
            }
            else
            {
                _equipment._clickedButtons.Add(this);
            
                WeaponClass _tempClass = _equipment._clickedButtons[0]._weaponClass;
                _tempClass.SetInUse(false);
                _equipment._clickedButtons[0]._weaponClass = _equipment._clickedButtons[1]._weaponClass;
                _equipment._clickedButtons[1]._weaponClass.SetInUse(true);
                _equipment._clickedButtons[1]._weaponClass = _tempClass;
                _equipment._clickedButtons[0].UpdateVisuals();
                _equipment._clickedButtons[1].UpdateVisuals();
            }
            _equipment._clickedButtons.Clear();
        }
    }
}
