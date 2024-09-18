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
        if (_up)
        {
            
        }
    }
}
