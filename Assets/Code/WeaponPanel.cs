using DefaultNamespace;
using TMPro;
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
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (_weaponClass == null) return;
        UpdateVisuals();
        CheckForUnlock();
        //if (!_weaponClass.CheckInUse()) _weaponImage.color = new Color32(255 / 2, 255 / 2, 225 / 2, 255);
    }

    public void UpdateVisuals()
    {
        if (_weaponClass == null) return;
        _weaponSprite = _weaponClass.GetWeaponSprite();

        CheckForUnlock();
    }

    public void CheckForUnlock()
    {
        if (_weaponClass == null) return;
        if (_weaponClass.IsWeaponUnlocked())
        {
            //_weaponImage.sprite = _weaponSprite;
            //_weaponImage.color = new Color32(255, 255, 225, 255);
            _titleText.text = _weaponClass.GetWeaponName();
            _descriptionText.text = _weaponClass.GetWeaponDescription();
            _button.interactable = true;
        }
        else
        {
            //_weaponImage.sprite = _lockedSprite;
            //_weaponImage.color = new Color32(255 / 2, 255 / 2, 225 / 2, 255);
            //_button.interactable = false;
            _titleText.text = "LOCKED";
            _descriptionText.text = "LOCKED";
        }
        /*
        if (!_weaponClass.CheckInUse())
            _weaponImage.color = new Color32(255 / 2, 255 / 2, 225 / 2, 255);
        else
            _weaponImage.color = new Color32(255, 255, 225, 255);
            */
    }

    private bool IsSameType(WeaponClass _weaponClass)
    {
        return this._weaponClass.GetWeaponType() == _weaponClass.GetWeaponType();
    }

    public void ChooseWeapon()
    {
        Debug.LogWarning("ChooseButton");
        if (!_weaponClass.IsWeaponUnlocked())
        {
            _gameManager._notyficationBaner.ShotMessage("Error", "This weapon is locked");
            return;
        }
        
        if (_weaponClass.CheckInUse())
        {
            if (_equipment._weaponsInUse.Count == 1)
            {
                _gameManager._notyficationBaner.ShotMessage("Error", "You can't remove last weapon!");
                return;
            }

            _equipment._weaponsInUse.Remove(_weaponClass);
            //_weaponImage.color = new Color32(255 / 2, 255 / 2, 225 / 2, 255);
        }
        else
        {
            if (_equipment._weaponsInUse.Count == 4)
            {
                _gameManager._notyficationBaner.ShotMessage("Error", "You reach weapons limit!");
                return;
            }

            _equipment._weaponsInUse.Add(_weaponClass);
            _gameManager._notyficationBaner.ShotMessage(_weaponClass.GetWeaponName(), 
                _weaponClass.GetWeaponDescription());
            //_weaponImage.color = new Color32(255, 255, 225, 255);
        }

        _equipment.RefleshWeaponAmountInfo();
        _weaponClass.SetInUse(!_weaponClass.CheckInUse());
    }
}