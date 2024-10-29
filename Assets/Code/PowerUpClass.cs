using UnityEngine;

public enum PowerUpsEnum
{
    Health,
    Regeneration,
    Damage,
    ReloadSped,
    Range,
    BulletDodge,
    FieldOfView,
    Luck
}

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PowerUPScriptableObject", order = 1)]
    public class PowerUpClass : ScriptableObject
    {
        [SerializeField] private string _descrtiption;
        [SerializeField] private PowerUpsEnum _type;
        [SerializeField] private float _playerBonus;
        [SerializeField] private Sprite _sprite;

        public PowerUpClass(PowerUpsEnum _type, string _descrtiption)
        {
            this._type = _type;
            this._descrtiption = _descrtiption;
        }

        public PowerUpsEnum GetPowerUpType()
        {
            return _type;
        }

        public string GetPowerUpDescription()
        {
            var _formatedDescrtiption = _descrtiption.Replace("_", _playerBonus.ToString());
            return _formatedDescrtiption;
        }

        public float GetPlayerBonus()
        {
            return _playerBonus / 100;
        }

        public Sprite GetPowerUpSprite()
        {
            return _sprite;
        }
    }
}