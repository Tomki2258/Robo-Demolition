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
    public Image _backgroundImage;
    private void Start()
    {
        _equipment = FindFirstObjectByType<EquipmentCanvas>();
        _gameManager = FindObjectOfType<GameManager>();
        if (_weaponClass == null) return;
        UpdateVisuals();
        CheckForUnlock();
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
            SwitchPanelVisuals(true);
            _titleText.text = _weaponClass.GetWeaponName();
            _descriptionText.text = _weaponClass.GetWeaponDescription();
            _button.interactable = true;
        }
        else
        {
            //_weaponImage.sprite = _lockedSprite;
            SwitchPanelVisuals(false);
            //_button.interactable = false;
            _titleText.text = "LOCKED";
            _descriptionText.text = "LOCKED";
        }

        if (_weaponClass.CheckInUse())
            SwitchPanelVisuals(true);
        else
            SwitchPanelVisuals(false);
    }

    private bool IsSameType(WeaponClass _weaponClass)
    {
        return this._weaponClass.GetWeaponType() == _weaponClass.GetWeaponType();
    }

    public void ChooseWeapon()
    {
        if (!_weaponClass.IsWeaponUnlocked())
        {
            _gameManager._notyficationBaner.ShotMessage("Error", "This weapon is locked",true,false);
            return;
        }
        
        if (_weaponClass.CheckInUse())
        {
            if (_equipment._weaponsInUse.Count == 1)
            {
                _gameManager._notyficationBaner.ShotMessage("Error", "You can't remove last weapon!",true,false);
                return;
            }
            SwitchPanelVisuals(false);
            _gameManager._notyficationBaner.ShotMessage("Weapon Removed", "",false,false);
            _equipment._weaponsInUse.Remove(_weaponClass);

        }
        else
        {
            if (_equipment._weaponsInUse.Count == 4)
            {
                _gameManager._notyficationBaner.ShotMessage("Error", "You reach weapons limit!",true,false);
                return;
            }
            SwitchPanelVisuals(true);
            _equipment._weaponsInUse.Add(_weaponClass);
            _gameManager._notyficationBaner.ShotMessage(_weaponClass.GetWeaponName(), 
                _weaponClass.GetWeaponDescription(),false,false);

        }

        _equipment.RefleshWeaponAmountInfo();
        _weaponClass.SetInUse(!_weaponClass.CheckInUse());
    }

    private void SwitchPanelVisuals(bool _isOn)
    {
        if (_isOn)
        {
            _backgroundImage.color = new Color(255, 255, 255, 100);
        }
        else
        {
            _backgroundImage.color = Color.gray;
        }
    }
}