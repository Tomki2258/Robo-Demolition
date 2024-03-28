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
        public string _desctiption;
        public string _rewardName;
        public RewardType _rewardType;

        public LevelUpPowerup(RewardType rewardType, string rewardName, string desctiption)
        {
            _rewardType = rewardType;
            _rewardName = rewardName;
            _desctiption = desctiption;
        }
    }
}