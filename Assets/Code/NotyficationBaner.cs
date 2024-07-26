using System.Collections;
using TMPro;
using UnityEngine;

public class NotyficationBaner : MonoBehaviour
{
    public GameObject _notyficationBaner;
    [SerializeField] private TMP_Text _title;
    [SerializeField] TMP_Text _message;

    [SerializeField] Animator animator;
    [SerializeField] string _titleText;
    public string _messageText;
    public int _waitTime;
    private Coroutine _currentEnumerator;
    public AnimationClip _animationClip;
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
        animator.SetTrigger("isOpen");
        yield return new WaitForSecondsRealtime(_animationClip.length);
        _notyficationBaner.SetActive(false);
    }

    public void CancelIEnumerator()
    {
        if (_currentEnumerator != null)
        {
            StopCoroutine(_currentEnumerator);
            _notyficationBaner.SetActive(false);
        }
    }
}