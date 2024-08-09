using System;
using TMPro;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    [SerializeField] private QuestClass _currentQuest;
    [SerializeField] private TMP_Text _currentTastValue;
    [SerializeField] private TMP_Text _targetTastValue;
    [SerializeField] private TMP_Text _questTypeText;
    private void Start()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void RefleshUI()
    {
        _questTypeText.text = _currentQuest.GetQuestType().ToString();
        _currentTastValue.text = _currentQuest.GetCurrentValue().ToString();
        _targetTastValue.text = _currentQuest.GetTargetValue().ToString();
    }

    public void SetQuest(QuestClass _questClass)
    {
        _currentQuest = _questClass;
        RefleshUI();
    }
}
