using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using Object = System.Object;
using Random = UnityEngine.Random;

public class QuestManager : MonoBehaviour
{
    public List<QuestClass> _activeQuestsList;
    private int _maxActiveQuests = 3;
    public QuestClass _questClassObject;
    private GameManager _gameManager;
    public int _questWaitTime; //Time in MINUTES
    [SerializeField] private GameObject _questPanelPrefab;
    [SerializeField] private Transform _questPanelUI;
    [SerializeField] private TMP_Text _newQuestTime;
    [Header("Quests Range Values")]
    public int _killedEnemiesRange;
    public int _collectedPowerUpsRange;
    public int _surviveTimeRange;
    public int _shootenBulletsRange;
    public long _targetTimeTicks;
    public long _currentTicks;
    private string _questPath; 
    private UserData _userData;
    private NotyficationBaner _notyficationBaner;
    private void Awake()
    {
        _notyficationBaner = FindObjectOfType<NotyficationBaner>();
        _userData = FindObjectOfType<UserData>();
        _questPath = Application.persistentDataPath + "/SavedQuests/";
        _gameManager = GetComponent<GameManager>();
        //DoQuest();
        LoadSavedQuests();
        CheckQuests();
    }

    private void FixedUpdate()
    {
        if(_gameManager._gameLaunched || _activeQuestsList.Count >= _maxActiveQuests)
        {
            _newQuestTime.gameObject.SetActive(false);
        }
        else
        {
            _newQuestTime.gameObject.SetActive(true);
            _newQuestTime.transform.SetSiblingIndex(_questPanelUI.childCount);
            DateTime _currentTime = DateTime.Now;
            _targetTimeTicks = new DateTime(GetSavedTime()).Ticks;
            TimeSpan _timeSpan = new DateTime(GetSavedTime()) - _currentTime;
            _currentTicks = _currentTime.Ticks;
            if (_currentTicks >= _targetTimeTicks)
            {
                DoQuest();
                SaveTime();
            }
            
            TimeSpan _timeLeast = new DateTime(_targetTimeTicks) - new DateTime(_currentTicks);

            _newQuestTime.text = $"New quest in\n{_timeLeast.Minutes + (_timeLeast.Hours * 60)}:" +
                                 $"{_timeLeast.Seconds}"; 
        }
    }
    private bool IsQuestDone(QuestClass _quest)
    {
        int _targetValue = _quest.GetTargetValue();
        int _currentValue = _quest.GetCurrentValue();
        return _targetValue > _currentValue;
    }
    private void LoadSavedQuests()
    {
        if (!System.IO.Directory.Exists(_questPath))
        {
            System.IO.Directory.CreateDirectory(_questPath);
        }
        string[] files = System.IO.Directory.GetFiles(_questPath, "*.json");

        foreach (string file in files)
        {
            string json = System.IO.File.ReadAllText(file);
            QuestClass quest = ScriptableObject.CreateInstance<QuestClass>();
            JsonUtility.FromJsonOverwrite(json, quest);
            if (IsQuestDone(quest))
            {
                _activeQuestsList.Add(quest);
                ShowQuest(quest);
            }
        }
    }

    public void SaveQuests()
    {
        Debug.LogWarning("SAVING QUESTS");
        foreach (QuestClass quest in _activeQuestsList)
        {
            string filePath = $"{_questPath}{quest.GetIndex()}.json";
            string json = JsonUtility.ToJson(quest);
            System.IO.File.WriteAllText(filePath, json);
        }
    }
    private void DoQuest()
    {
        if(_activeQuestsList.Count >= _maxActiveQuests) return;
        
        QuestClass _newQuest = ScriptableObject.CreateInstance<QuestClass>();
        _newQuest.SetIndex(
            Random.Range(0,100));
        QuestType _randomQuestType 
            = Enum.GetValues(typeof(QuestType)).Cast<QuestType>().ToList()[Random.Range(0,_newQuest.GetQuestsTypesLength())];
            
        int _questTargetValue = 0;
        switch (_randomQuestType)
        {
            case QuestType.killEnemies:
                _questTargetValue = GetRandomQuestValue(_killedEnemiesRange);
                break;
            case QuestType.collectPowerUps:
                _questTargetValue = GetRandomQuestValue(_collectedPowerUpsRange);
                break;
            //case QuestType.surviveTime:
                //_questTargetValue = GetRandomQuestValue(_surviveTimeRange);
                break;
            case QuestType.shootenBullets:
                _questTargetValue = GetRandomQuestValue(_shootenBulletsRange);
                break;
        }
        _newQuest.CreateQuest(_questTargetValue,
            _randomQuestType,
            10,
            _newQuest.GetIndex());
        
        string __questPath = _questPath + _newQuest.GetIndex() + ".json";
        
        string _soJson = JsonUtility.ToJson(_newQuest);
        
        System.IO.File.WriteAllText(__questPath,_soJson);
        
        _activeQuestsList.Add(_newQuest);
        
        ShowQuest(_newQuest);
    }
    private int GetRandomQuestValue(int _range)
    {
        return Random.Range(_range/3,_range);
    }
    private void ShowQuest(QuestClass _questClass)
    {
        GameObject _quest = Instantiate(_questPanelPrefab, transform.position, Quaternion.identity);
        QuestPanel _currentQuestPanel = _quest.GetComponent<QuestPanel>();
        _currentQuestPanel.SetQuest(_questClass);
        //_quest.transform.localScale = new Vector3(1, 1, 1);
        _quest.transform.parent = _questPanelUI;
    }
    public void CheckQuests()
    {
        foreach (QuestClass _quest in _activeQuestsList.ToList())
        {
            if(_quest.IsQuestDone()) return;
         
            Debug.LogWarning(_quest.GetQuestType());
            if (_quest.IsQuestCompleted())
            {
                _quest.CompleteQuest();
                
                int _questReward = _quest.GetQuestRewardAmount();
                _userData.AddPlayerCoins(_questReward);
                
                int _listIndex = _activeQuestsList.IndexOf(_quest);
                _activeQuestsList.RemoveAt(_listIndex);
                
                _notyficationBaner.ShotMessage("Quest Completed",
                    _questReward.ToString() + " coins earned",
                    false,
                    true);
            }
        }
    }

    public void SaveTime()
    {
        DateTime _targetTime = DateTime.Now.AddMinutes(_questWaitTime);
        long _ticks = _targetTime.Ticks;
        PlayerPrefs.SetString("LastSavedTime",_ticks.ToString());
    }

    private long GetSavedTime()
    {
        string _ticksString = PlayerPrefs.GetString("LastSavedTime");
        if(string.IsNullOrEmpty(_ticksString))
        {
            SaveTime(); 
        }
        return Convert.ToInt64(_ticksString);
    }
}
