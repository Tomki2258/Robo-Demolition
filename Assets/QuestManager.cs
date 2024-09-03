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
    private void Start()
    {
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
            DateTime _savedTime = new DateTime(GetSavedTime());
            TimeSpan _timeSpan = _savedTime - _currentTime;
            int _elapsedTime = Math.Abs(_timeSpan.Minutes);
            
            if (_elapsedTime > _questWaitTime)
            {
                DoQuest();
            }    
            
            int _remainingMinutes = GetRemainingTimeForNewQuest() / 60;
            int _remainingSeconds = GetRemainingTimeForNewQuest() - (_remainingMinutes * 60);
            _newQuestTime.text = $"New quest in\n{_remainingMinutes}:{_remainingSeconds}";
        }
    }
    
    public int GetRemainingTimeForNewQuest()
    {
        DateTime currentTime = DateTime.Now;
        DateTime savedTime = new DateTime(GetSavedTime());
        TimeSpan timeSpan = savedTime - currentTime;
        int elapsedTime = Math.Abs(timeSpan.Seconds);
        return _questWaitTime * 60  - elapsedTime;
    }
    private void LoadSavedQuests()
    {
        List<QuestClass> _loadedQuests = Resources.LoadAll("SavedQuests", typeof(QuestClass)).Cast<QuestClass>().ToList();
        foreach (QuestClass _quest in _loadedQuests)
        {
            Debug.LogWarning(_quest.GetQuestType());
            if (!_quest.IsQuestDone())
            {
                _activeQuestsList.Add(_quest);
                ShowQuest(_quest);
            }
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
        _newQuest.CreateQuest(Random.Range(5,10),_randomQuestType,10);
        AssetDatabase.CreateAsset(_newQuest,$"Assets/Resources/SavedQuests/{_newQuest.GetIndex()}.asset");
        _activeQuestsList.Add(_newQuest);
        
        ShowQuest(_newQuest);
        
        SaveTime();
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
        if(_ticksString == "")
        {
            SaveTime(); 
        }
        return Convert.ToInt64(_ticksString);
    }
}
