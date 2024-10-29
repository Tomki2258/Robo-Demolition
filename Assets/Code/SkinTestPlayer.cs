using System;
using UnityEngine;

public class SkinTestPlayer : MonoBehaviour
{
    public PlayerSkinSO _currentSelectedSkin;
    
    [Header("Garage Player")] [SerializeField]
    private Animator _garageAnimator;
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
}
