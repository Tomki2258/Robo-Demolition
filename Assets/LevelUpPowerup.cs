using UnityEngine;

namespace DefaultNamespace
{
    public enum RewardType
    {
        Health,
        Damage,
        Speed,
        Regeneration,
        ReloadSpeed
    }
    public class LevelUpPowerup
    {
        public LevelUpPowerup(RewardType rewardType, string rewardName, string desctiption)
        {
            Debug.LogWarning("Power up created");
            _rewardType = rewardType;
            _rewardName = rewardName;
            _desctiption = desctiption;
        }
        public RewardType _rewardType;
        public string _rewardName;
        public string _desctiption;
    }
}