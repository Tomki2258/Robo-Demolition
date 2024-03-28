using UnityEngine;

namespace DefaultNamespace
{
    public enum PowerUpsEnum
    {
        Health,
        Regeneration,
        Damage,
        ReloadSped,
        Range
    }
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PowerUPScriptableObject", order = 1)]
    public class PowerUpClass : ScriptableObject
    {
        private string _descrtiption;
        private PowerUpsEnum _type;
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
            return _descrtiption;
        }
    }
}