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
    SphereAttack,
    MineDeployer,
    GranadeLauncher
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
        [SerializeField] private float _damage;
        [SerializeField] private float _reloadTimeMax;
        [SerializeField] private float _attackRange;
        public float _currentReloadTime;

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

        public float GetDamage()
        {
            return _damage;
        }

        public float GetReloadTime()
        {
            return _reloadTimeMax;
        }

        public float GetAttackRange()
        {
            return _attackRange;
        }

        public bool CheckShoot()
        {
            return _currentReloadTime > _reloadTimeMax;
        }
}
}