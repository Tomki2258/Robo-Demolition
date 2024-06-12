using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameSettings : MonoBehaviour
{
    [Header("Graphics")]
    public bool _qualityOn;
    public Sprite _qualityOnSprite;
    public Sprite _qualityOffSprite;
    public Image _qualityImage;
    [Header("Audio")]
    public AudioListener _audioListener;
    public Sprite _audioOnSprite;
    public Sprite _audioOffSprite;
    public Image _audioImage;
    [Header("Settings")] 
    public String _twitterURL;
    public String _facebookURL;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        var _savedQuality = Convert.ToBoolean(PlayerPrefs.GetInt("SavedQuality"));
        LoadQuality(_savedQuality);
        var _savedAudio = Convert.ToBoolean(PlayerPrefs.GetInt("SavedAudio"));
        LoadAudio(_savedAudio);
    }

    private void LoadQuality(bool _high)
    {
        QualitySettings.vSyncCount = 1;
        _qualityOn = _high;
        if (_high)
        {
            QualitySettings.SetQualityLevel(2);
            _qualityImage.sprite = _qualityOnSprite;
        }
        else
        {
            QualitySettings.SetQualityLevel(0);
            _qualityImage.sprite = _qualityOffSprite;
        }
        PlayerPrefs.SetInt("SavedQuality", _high ? 1 : 0);
        Application.targetFrameRate = 60;
    }
    
    public void SwitchQualitySettings()
    {
        _qualityOn = !_qualityOn;
        if (_qualityOn)
        {
            QualitySettings.SetQualityLevel(4);
            _qualityImage.sprite = _qualityOnSprite;
            
        }
        else
        {
            QualitySettings.SetQualityLevel(0);
            _qualityImage.sprite = _qualityOffSprite;
        }

        PlayerPrefs.SetInt("SavedQuality", _qualityOn ? 1 : 0);
    }
    private void LoadAudio(bool _enabled)
    { 
        _audioListener.enabled = _enabled;
        if (_enabled)
        {
            _audioImage.sprite = _audioOnSprite;
        }
        else
        {
            _audioImage.sprite = _audioOffSprite;
        }
        PlayerPrefs.SetInt("SavedAudio", _enabled ? 1 : 0);
    }
    public void SwitchAudioSettings()
    {
        if (_audioListener.enabled)
        {
            _audioListener.enabled = false;
            _audioImage.sprite = _audioOnSprite;
        }
        else
        {
            _audioListener.enabled = true;
            _audioImage.sprite = _audioOffSprite;
        }
        PlayerPrefs.SetInt("SavedAudio", _audioListener.enabled ? 1 : 0);
    }

    public void LaunchURL(int _URLindex)
    {
        switch (_URLindex)
        {
            case 0:
                Application.OpenURL(_twitterURL);
                break;
            default:
                break;
        }
    }
}
