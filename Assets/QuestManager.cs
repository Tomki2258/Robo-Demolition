using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<UnityEngine.Object> _activeQuestsList;
    private int _maxActiveQuests = 3;
    public QuestClass _questClassObject;

    private void Start()
    {
        LoadSavedQuests();
    }

    private void LoadSavedQuests()
    {
        _activeQuestsList = Resources.LoadAll("SavedQuests", typeof(QuestClass)).ToList();
   }
    private void DoQuest()
    {
        if(_activeQuestsList.Count >= _maxActiveQuests) return;
        
        QuestClass _newQuest = ScriptableObject.CreateInstance<QuestClass>();
        _newQuest.SetIndex(
            Random.Range(0,100));
        AssetDatabase.CreateAsset(_newQuest,$"Assets/SavedQuests/{_newQuest.GetIndex()}.asset");
        _activeQuestsList.Add(_newQuest);
    }
}
