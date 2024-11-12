using System;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public enum QuestType{
    killEnemies,
    //walkDistance,
    //surviveTime,
    shootenBullets,
    collectPowerUps
}

[CreateAssetMenu(fileName = "QuestClass", menuName = "ScriptableObjects/QuestClass")]
public class QuestClass : ScriptableObject
{
    [SerializeField] private int _currentValue;
    [SerializeField] private int _targetValue;
    [SerializeField] private QuestType _questType;
    [SerializeField] private int _questRewardAmount;
    [SerializeField] private int _randomIndex;
    public bool _questDone;
    public void CreateQuest(int _targetValue,QuestType _questType,int _questRewardAmount,int _randomIndex)
    {
        this._targetValue = _targetValue;
        this._questType = _questType;
        this._questRewardAmount = _questRewardAmount;
        this._randomIndex = _randomIndex;
    }

    public void AddValue(int _value)
    {
        _currentValue += _value;
    }
    
    public bool IsQuestCompleted()
    {
        if (_currentValue >= _targetValue)
        {
            _questDone = true;
            return true;
        }

        return false;
    }

    public void SetIndex(int _index)
    {
        this._randomIndex = _index;
    }
    public int GetIndex()
    {
        return _randomIndex;
    }

    public QuestType GetQuestType()
    {
        return this._questType;
    }

    public bool IsQuestDone()
    {
        return _questDone;
    }

    public int GetCurrentValue()
    {
        return _currentValue;
    }

    public int GetTargetValue()
    {
        return _targetValue;
    }
    public int GetQuestsTypesLength()
    {
        return Enum.GetValues(typeof(QuestType)).Length;
    }

    public string GetQuestDescription()
    {
        switch(_questType)
        {
            case QuestType.killEnemies:
                return "Enemies killed";
            //case QuestType.walkDistance:
            //    return "Distance traveled";
            //case QuestType.surviveTime:
                //return "Survive time";
            case QuestType.shootenBullets:
                return "Bullets shot";
            case QuestType.collectPowerUps:
                return "Power ups collected";
            default:
                return "No description";
        }
    }

    public void CompleteQuest()
    {
        _currentValue = _targetValue + 1;
        _questDone = true;
    }
    public int GetQuestRewardAmount()
    {
        return _questRewardAmount;
    }
}
