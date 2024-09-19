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

    private void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        SetSkinChangeUI();
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
    }
}
