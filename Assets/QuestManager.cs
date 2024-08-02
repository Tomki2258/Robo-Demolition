using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using Object = System.Object;
using Random = UnityEngine.Random;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<QuestClass> _activeQuestsList;
    private int _maxActiveQuests = 3;
    public QuestClass _questClassObject;
    private GameManager _gameManager;
    public int _questWaitTime; //Time in MINUTES
    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
        //DoQuest();
        LoadSavedQuests();
        CheckQuests();
    }

    private void FixedUpdate()
    {
        if(_gameManager._gameLaunched || _activeQuestsList.Count >= _maxActiveQuests) return;
        
        DateTime _currentTime = DateTime.Now;
        DateTime _savedTime = new DateTime(GetSavedTime());
        TimeSpan _timeSpan = _savedTime - _currentTime;

        int _elapsedTime = Math.Abs(_timeSpan.Minutes);
        //Debug.LogWarning($"{_timeSpan.Seconds}");
        if (Math.Abs(_elapsedTime) > _questWaitTime)
        {
            DoQuest();
        }
    }

    private void LoadSavedQuests()
    {
        List<QuestClass> _loadedQuests = Resources.LoadAll("SavedQuests", typeof(QuestClass)).Cast<QuestClass>().ToList();
        foreach (QuestClass _quest in _loadedQuests)
        {
            Debug.LogWarning(_quest.GetQuestType());
            if (_quest.IsQuestDone())
            {
                //_activeQuestsList.Add(_quest);
            }
            _activeQuestsList.Add(_quest);

        }
   }
    private void DoQuest()
    {
        if(_activeQuestsList.Count >= _maxActiveQuests) return;
        
        QuestClass _newQuest = ScriptableObject.CreateInstance<QuestClass>();
        _newQuest.SetIndex(
            Random.Range(0,100));
        AssetDatabase.CreateAsset(_newQuest,$"Assets/Resources/SavedQuests/{_newQuest.GetIndex()}.asset");
        _activeQuestsList.Add(_newQuest);
        
        SaveTime();
    }

    private void CheckQuests()
    {
        foreach (QuestClass _quest in _activeQuestsList.ToList())
        {
            if(_quest.IsQuestDone()) return;
            
            bool _questResult = _quest.IsQuestCompleted();
            if (_questResult)
            {
                int _listIndex = _activeQuestsList.IndexOf(_quest);
                _activeQuestsList.RemoveAt(_listIndex);
            }
        }
    }

    public void SaveTime()
    {
        DateTime _now = DateTime.Now;
        long _ticks = _now.Ticks;
        PlayerPrefs.SetString("LastSavedTime",_ticks.ToString());
    }

    private long GetSavedTime()
    {
        string _ticksString = PlayerPrefs.GetString("LastSavedTime");
        return Convert.ToInt64(_ticksString);
    }
}
