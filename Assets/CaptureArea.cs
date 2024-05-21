using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureArea : MonoBehaviour
{
    public bool _playerIn;
    public int _maxTime;
    public float _currentTime;
    private UIManager _uiManager;
    private Image _captureAreaImage;
    private void Start()
    {
        _uiManager = FindAnyObjectByType<UIManager>();
        _captureAreaImage = _uiManager._captureAreaImage;
        Destroy(gameObject,60);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _captureAreaImage.enabled = true;
            _playerIn = true;
        }
    }

    private void FixedUpdate()
    {
        if (_playerIn)
        {
            if (_currentTime < _maxTime)
            {
                _currentTime += Time.deltaTime;
            }
            else
            {
                CompleteArea();
            }
        }
        else
        {
            if (_currentTime < _maxTime && _currentTime > 0)
            {
                _currentTime -= Time.deltaTime / 3;
            }
        }

        if (_captureAreaImage.enabled)
        {
            _captureAreaImage.fillAmount = _currentTime / _maxTime;
        }
    }

    private void CompleteArea()
    {
        FindFirstObjectByType<GameManager>().GetCaptureAreaReward();
        Destroy(gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerIn = false;
        }
    }

    private void OnDestroy()
    {
        _captureAreaImage.enabled = false;
    }
}
