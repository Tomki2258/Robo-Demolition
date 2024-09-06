using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{
    [SerializeField] private QuestClass _currentQuest;
    [SerializeField] private TMP_Text _currentTastValue;
    [SerializeField] private TMP_Text _targetTastValue;
    [SerializeField] private TMP_Text _questTypeText;
    [SerializeField] private Slider _questValueSlider;
    private void Start()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void RefleshUI()
    {
        _questTypeText.text = _currentQuest.GetQuestDescription();
        _currentTastValue.text = _currentQuest.GetCurrentValue().ToString();
        _targetTastValue.text = _currentQuest.GetTargetValue().ToString();
        _questValueSlider.maxValue = _currentQuest.GetTargetValue();
        _questValueSlider.value = _currentQuest.GetCurrentValue();
    }

    public void SetQuest(QuestClass _questClass)
    {
        _currentQuest = _questClass;
        RefleshUI();
    }
}
