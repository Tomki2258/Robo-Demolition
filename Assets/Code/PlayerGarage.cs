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
    private PlayerMovement _playerMovement;
    [SerializeField] private GameObject _playerGarage;
    [SerializeField] private PlayerSkinSO _currentSelectedSkin;
    private UIManager _uiManager;

    [Header("Real Player")] [SerializeField]
    private GameObject _realBody;
    [SerializeField] private GameObject _realHead;
    [SerializeField] private GameObject _realCircle;
    [SerializeField] private GameObject _beltCircle;
    [SerializeField] private GameObject _realHands;
    [SerializeField] private GameObject _realLaserGun;
    [SerializeField] private GameObject _realLeftGun;
    [SerializeField] private GameObject _realRightGun;
    [SerializeField] private GameObject _realLegs;
    [SerializeField] private GameObject _realRocketGun;
    [SerializeField] private GameObject _realSphereAttack;

    [Header("Garage Player")] [SerializeField]
    private Animator _garageAnimator;
    [SerializeField] private GameObject _garageBody;
    [SerializeField] private GameObject _garageHead;
    [SerializeField] private GameObject _garageCircle;
    [SerializeField] private GameObject _garageBeltCircle;
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
        //gameObject.SetActive(false);
        _playerGarage.SetActive(false);
        _playerMovement = _realPlayer.GetComponent<PlayerMovement>();
        
        LoadSavedSkin();
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
        switch (_up)
        {
            case true:
                if (_currentIndex == _playerSkins.Count - 1)
                {
                    _currentSelectedSkin = _playerSkins[0];
                }
                else
                {
                    _currentSelectedSkin = _playerSkins[_currentIndex + 1];
                }
                break;
            case false:
                if (_currentIndex == 0)
                {
                    _currentSelectedSkin = _playerSkins[_playerSkins.Count - 1];
                }
                else
                {
                    _currentSelectedSkin = _playerSkins[_currentIndex - 1];
                }
                break;
                
        }
            
        SetSkinChangeUI();
    }

    private void SetSkinChangeUI()
    {
        _uiManager._lockedSkinBaner.SetActive(!_currentSelectedSkin.IsUnlocked());
        _uiManager._buySkinButton.SetActive(!_currentSelectedSkin.IsUnlocked());
        _uiManager._selectSkinButton.SetActive(_currentSelectedSkin.IsUnlocked());
        
        _uiManager._skinNameText.text = _currentSelectedSkin.GetSkinName();
        _uiManager._skinPriceText.text = _currentSelectedSkin.GetSkinPrice().ToString();
        
        int _currentPlayerCoins = _uiManager._userData.GetPlayerCoins();
        
        _garageBody.GetComponent<MeshRenderer>().material = _currentSelectedSkin._bodyMaterial;
        _garageHead.GetComponent<MeshRenderer>().material = _currentSelectedSkin._headMaterial;
        _garageCircle.GetComponent<MeshRenderer>().material = _currentSelectedSkin._circleGunmaterial;
        _garageBeltCircle.GetComponent<MeshRenderer>().material = _currentSelectedSkin._circleBeltMaterial;
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
        _playerMovement._currentPlayerSkin = _currentSelectedSkin;
        SaveCurrentSkin();
        
        _realBody.GetComponent<MeshRenderer>().material = _currentSelectedSkin._bodyMaterial;
        _realHead.GetComponent<MeshRenderer>().material = _currentSelectedSkin._headMaterial;
        _realCircle.GetComponent<MeshRenderer>().material = _currentSelectedSkin._circleGunmaterial;
        _beltCircle.GetComponent<MeshRenderer>().material = _currentSelectedSkin._circleBeltMaterial;
        _realHands.GetComponent<MeshRenderer>().material = _currentSelectedSkin._handsMaterial;
        _realLaserGun.GetComponent<MeshRenderer>().material = _currentSelectedSkin._laserGunMaterial;
        _realLeftGun.GetComponent<MeshRenderer>().material = _currentSelectedSkin._leftGunMaterial;
        _realRightGun.GetComponent<MeshRenderer>().material = _currentSelectedSkin._rightGunMaterial;
        _realLegs.GetComponent<SkinnedMeshRenderer>().material = _currentSelectedSkin._legsMaterial;
        _realRocketGun.GetComponent<MeshRenderer>().material = _currentSelectedSkin._rocketGunMaterial;
        _realSphereAttack.GetComponent<MeshRenderer>().material = _currentSelectedSkin._spheareGunMaterial;
    }

    private void LoadSavedSkin()
    {
        int _loadedSkinIndex = PlayerPrefs.GetInt("CurrentSkin");
        _currentSelectedSkin = _playerSkins[_loadedSkinIndex];
        SetSkinChangeUI();
        ApplySkinForPlayer();
    }
    private void SaveCurrentSkin()
    {
        PlayerPrefs.SetInt("CurrentSkin",_playerSkins.IndexOf(_currentSelectedSkin));
    }

    public void BuySkin()
    {
        int _skinPrice = _currentSelectedSkin.GetSkinPrice();
        int _currentPlayerCoins = _uiManager._userData.GetPlayerCoins();
        if (_currentPlayerCoins >= _skinPrice)
        {
            _uiManager._userData.AddPlayerCoins(-_skinPrice);
            _currentSelectedSkin.UnlockSkin();
            SetSkinChangeUI();
            
            _uiManager._garageCoins.text = _currentPlayerCoins.ToString();
        }
        else
        {
            Debug.LogWarning("NOT ENOUGH COINS");
        }
    }
}
