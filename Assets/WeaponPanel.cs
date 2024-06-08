using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class WeaponPanel : MonoBehaviour
{
    public WeaponClass _weaponClass;
    [SerializeField] private Image _weaponImage;
    public Sprite _weaponSprite;
    public Sprite _lockedSprite;
    public bool _isUnlocked;
    void Start()
    {
        _weaponImage.sprite = _weaponSprite; //comment
    }

    public void CheckForUnlock()
    {
        if (_weaponClass.IsWeaponUnlocked())
        {
            _weaponImage.sprite = _weaponSprite;
        }
        else
        {
            _weaponImage.sprite = _lockedSprite;
        }
    }
}
