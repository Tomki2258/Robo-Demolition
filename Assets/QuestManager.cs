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

    private void Start()
    {
        //DoQuest();
        LoadSavedQuests();
        CheckQuests();
    }

    private void LoadSavedQuests()
    {
        List<QuestClass> _loadedQuests = Resources.LoadAll("SavedQuests", typeof(QuestClass)).Cast<QuestClass>().ToList();
        foreach (QuestClass _quest in _loadedQuests)
        {
            if (!_quest.IsQuestDone())
            {
                _activeQuestsList.Add(_quest);
            }
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
}
