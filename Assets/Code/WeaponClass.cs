using UnityEngine;
public enum WeaponTypes
{
    Standard,
    MachineGun,
    Shotgun,
    LaserGum,
    RocketLauncher,
    OrbitalGun,
    SniperGun,
    CircleGun,
    SphereAttack
}
namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponsScriptableObject", order = 1)]
    public class WeaponClass : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private WeaponTypes _type;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private bool _isUnlocked;
        [SerializeField] private bool _isInUse;
        public string GetWeaponName()
        {
            return _name;
        }
        public WeaponTypes GetWeaponType()
        {
            return _type;
        }
        public string GetWeaponDescription()
        {
            return _description;
        }

        public Sprite GetWeaponSprite()
        {
            return _sprite;
        }
        public bool IsWeaponUnlocked()
        {
            return _isUnlocked;
        }

        public void UnlockWeapon()
        {
            _isUnlocked = true;
        }

        public bool CheckInUse()
        {
            return _isInUse;
        }
        public void SetInUse(bool _inUse)
        {
            _isInUse = _inUse;
        }
        public void SetUnlocked(bool _unlocked)
        {
            _isUnlocked = _unlocked;
        }

        public bool CheckForUse()
        {
            return _isInUse;
        }
    }
}