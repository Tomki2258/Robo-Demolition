using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGarage : MonoBehaviour
{
    [SerializeField]private List<Transform> _rotateElements;
    [SerializeField] private int _rotateSpeedScale;
    public bool _garageEnabled = false;
    [SerializeField] private List<PlayerSkinSO> _playerSkins;
    [SerializeField] private GameObject _realPlayer;
    [SerializeField] private GameObject _playerGarage;
    [SerializeField] private PlayerSkinSO _currentSelectedSkin;
    private UIManager _uiManager;

    [Header("Real Player")] [SerializeField]
    private GameObject _realBody;
    [SerializeField] private GameObject _realHead;
    [Header("Garage Player")] 
    [SerializeField] private GameObject _garageBody;
    [SerializeField] private GameObject _garageHead;
    [SerializeField] private GameObject _garageCircle;
    [SerializeField] private GameObject _garageHands;
    [SerializeField] private GameObject _garageLaserGun;
    [SerializeField] private GameObject _garageLeftGun;
    [SerializeField] private GameObject _garageRightGun;
    [SerializeField] private GameObject _garageLegs;
    [SerializeField] private GameObject _garageRocketGun;
    [SerializeField] private GameObject _sphereAttack;
    private void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        SetSkinChangeUI();
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if(!_garageEnabled) return;
        RotateElements();
    }

    private void RotateElements()
    {
        foreach (Transform _element in _rotateElements)
        {
            _element.Rotate(0, _rotateSpeedScale * Time.deltaTime, 0);
        }
    }
    
    public void SwitchSkinButton(bool _up)
    {
        int _currentIndex = _playerSkins.IndexOf(_currentSelectedSkin);
        if (_up)
        {
            if (_currentIndex == _playerSkins.Count - 1)
            {
                _currentSelectedSkin = _playerSkins[0];
            }
            else
            {
                _currentSelectedSkin = _playerSkins[_currentIndex + 1];
            }
        }
        else
        {
            if (_currentIndex == 0)
            {
                _currentSelectedSkin = _playerSkins[_playerSkins.Count - 1];
            }
            else
            {
                _currentSelectedSkin = _playerSkins[_currentIndex - 1];
            }
        }
            
        SetSkinChangeUI();
    }

    private void SetSkinChangeUI()
    {
        _uiManager._skinNameText.text = _currentSelectedSkin.GetSkinName();
        _uiManager._skinPriceText.text = _currentSelectedSkin.GetSkinPrice().ToString();
        
        int _currentPlayerCoins = _uiManager._userData.GetPlayerCoins();
        _garageBody.GetComponent<MeshRenderer>().material = _currentSelectedSkin._bodyMaterial;
        _garageHead.GetComponent<MeshRenderer>().material = _currentSelectedSkin._headMaterial;
        _garageCircle.GetComponent<MeshRenderer>().material = _currentSelectedSkin._circleGunmaterial;
        _garageHands.GetComponent<MeshRenderer>().material = _currentSelectedSkin._handsMaterial;
        _garageLaserGun.GetComponent<MeshRenderer>().material = _currentSelectedSkin._laserGunMaterial;
        _garageLeftGun.GetComponent<MeshRenderer>().material = _currentSelectedSkin._leftGunMaterial;
        _garageRightGun.GetComponent<MeshRenderer>().material = _currentSelectedSkin._rightGunMaterial;
        _garageLegs.GetComponent<SkinnedMeshRenderer>().material = _currentSelectedSkin._legsMaterial;
        _garageRocketGun.GetComponent<MeshRenderer>().material = _currentSelectedSkin._rocketGunMaterial;
        _sphereAttack.GetComponent<MeshRenderer>().material = _currentSelectedSkin._spheareGunMaterial;
    }

    public void ApplySkinForPlayer()
    {
        throw new NotImplementedException();
    }
}
