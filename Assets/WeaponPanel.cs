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
    [SerializeField] private Button _button;
    private GameManager _gameManager;
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
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
            _weaponImage.color = new Color32(255,255,225,255);
            _button.interactable = true;
        }
        else
        {
            _weaponImage.sprite = _lockedSprite;
            _weaponImage.color = new Color32(255/2,255/2,225/2,255);
            _button.interactable = false;
        }
    }

    private bool IsSameType(WeaponClass _weaponClass)
    {
        return this._weaponClass.GetWeaponType() == _weaponClass.GetWeaponType();
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
            if(_equipment._weaponPlaces.Contains(this) || IsSameType(_equipment._clickedButtons[0]._weaponClass) || _equipment.CheckForWeaponPanel(this._weaponClass))
            {
                _gameManager._notyficationBaner.ShotMessage("Error","You can't change weapon type !");
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
