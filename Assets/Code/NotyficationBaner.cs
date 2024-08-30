using System;
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
    [SerializeField] private AudioClip _errorAudioClip;
    [SerializeField] private AudioClip _clickAudioClip;
    [SerializeField] private AudioSource _audioSource;
    private float _startPosition;

    private void Start()
    {
        _startPosition = _notyficationBaner.transform.position.y;
    }

    public void ShotMessage(string title, string message,bool _isError,bool _onTop)
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

        _audioSource.PlayOneShot(_isError ? _errorAudioClip : _clickAudioClip);

        float _currentYValue = _startPosition;
        if (_onTop)
        {
            _currentYValue = 670;
        }
        _notyficationBaner.transform.position = new Vector3(_notyficationBaner.transform.position.x,
            _currentYValue,
            _notyficationBaner.transform.position.z);
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