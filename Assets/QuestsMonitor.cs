using UnityEngine;

[RequireComponent(typeof(QuestManager))]
public class QuestsMonitor : MonoBehaviour
{
    [SerializeField] private QuestManager _questManager;
    
    [Header("Currnt session stats")]
    public int _colledtedPowerUps;
    public int _killedEnemies;
    public int _bulletsShot;
    public int _timeSurvived;
    
    public void ApplyStats()
    {
        foreach (QuestClass _quest in _questManager._activeQuestsList)
        {
            switch (_quest.GetQuestType())
            {
                case QuestType.killEnemies:
                    _quest.AddValue(_killedEnemies);
                    break;
                case QuestType.collectPowerUps:
                    _quest.AddValue(_colledtedPowerUps);
                    break;
                case QuestType.shootenBullets:
                    _quest.AddValue(_bulletsShot);
                    break;
                case QuestType.surviveTime:
                    _quest.AddValue(_timeSurvived);
                    break;
            }
        }
        _questManager.CheckQuests();
    }
}
