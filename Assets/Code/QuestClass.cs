using System;using UnityEngine;
using Random = UnityEngine.Random;

public enum QuestType{
    killEnemies,
    walkDistance,
    surviveTime,
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
    private int _randomIndex;
    public void CreateQuest(int _targetValue,QuestType _questType,int _questRewardAmount)
    {
        this._targetValue = _targetValue;
        this._questType = _questType;
        this._questRewardAmount = _questRewardAmount;
    }

    public void AddValue(int _value)
    {
        _currentValue += _currentValue;
    }
    
    public bool IsQuestCompleted()
    {
        if (_currentValue >= _targetValue) return true;
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
}
