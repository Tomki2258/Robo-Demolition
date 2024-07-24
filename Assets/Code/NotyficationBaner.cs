using System.Collections;
using TMPro;
using UnityEngine;

public class NotyficationBaner : MonoBehaviour
{
    public GameObject _notyficationBaner;
    public TMP_Text _title;
    public TMP_Text _message;
    public string _titleText;
    public string _messageText;
    public int _waitTime;
    private Coroutine _currentEnumerator;
    public void ShotMessage(string title, string message)
    {
        _titleText = title;
        _messageText = message;
        if (_currentEnumerator == null)
        {
            _currentEnumerator = StartCoroutine(Show());
        }
        else
        {
            StopCoroutine(_currentEnumerator);
            _currentEnumerator = StartCoroutine(Show());
        }
    }

    private IEnumerator Show()
    {
        _notyficationBaner.SetActive(true);
        _title.text = _titleText;
        _message.text = _messageText;
        yield return new WaitForSecondsRealtime(_waitTime);
        _notyficationBaner.SetActive(false);
    }
}