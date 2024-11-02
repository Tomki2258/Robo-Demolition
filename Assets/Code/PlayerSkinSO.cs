using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkinSO", menuName = "Scriptable Objects/PlayerSkinSO")]
public class PlayerSkinSO : ScriptableObject
{
    [SerializeField] private int _skinPrice;
    [SerializeField] private string _skinName;
    public Material _bodyMaterial;
    public Material _circleGunmaterial;
    public Material _circleBeltMaterial;
    public Material _handsMaterial;
    public Material _headMaterial;
    public Material _laserGunMaterial;
    public Material _leftGunMaterial;
    public Material _rightGunMaterial;
    public Material _legsMaterial;
    public Material _rocketGunMaterial;
    public Material _spheareGunMaterial;
    [SerializeField] private bool _isUnlocked;
    public int GetSkinPrice()
    {
        return _skinPrice;
    }
    public string GetSkinName()
    {
        return _skinName;
    }

    public bool IsUnlocked()
    {
        return _isUnlocked;
    }

    public void CheckForUnlocked()
    {
        int _read = PlayerPrefs.GetInt(_skinName);
        if (_read == 1)
        {
            _isUnlocked = true;
        }
    }

    public void UnlockSkin()
    {
        PlayerPrefs.SetInt(_skinName, 1);
    }
}
