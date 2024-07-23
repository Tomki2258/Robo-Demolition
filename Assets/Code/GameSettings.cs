using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [Header("Graphics")] public bool _qualityOn;

    public Sprite _qualityOnSprite;
    public Sprite _qualityOffSprite;
    public Image _qualityImage;
    public Volume _postProcessVolume;
    [Header("Audio")] public AudioListener _audioListener;
    public bool _audioOn;
    public Sprite _audioOnSprite;
    public Sprite _audioOffSprite;
    public Image _audioImage;

    [Header("Settings")] public string _twitterURL;

    public string _facebookURL;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        var _savedQuality = Convert.ToBoolean(PlayerPrefs.GetInt("SavedQuality"));
        LoadQuality(_savedQuality);
        var _savedAudio = Convert.ToBoolean(PlayerPrefs.GetInt("SavedAudio"));
        //LoadAudio(_savedAudio);
    }

    private void LoadQuality(bool _high)
    {
        QualitySettings.vSyncCount = 1;
        _qualityOn = _high;
        if (_high)
        {
            QualitySettings.SetQualityLevel(2);
            _qualityImage.sprite = _qualityOnSprite;
            _postProcessVolume.enabled = true;
        }
        else
        {
            QualitySettings.SetQualityLevel(0);
            _qualityImage.sprite = _qualityOffSprite;
            _postProcessVolume.enabled = false;
        }

        PlayerPrefs.SetInt("SavedQuality", _high ? 1 : 0);
        Application.targetFrameRate = 60;
    }

    public void SwitchQualitySettings()
    {
        Debug.Log("Switch Quality");
        _qualityOn = !_qualityOn;
        if (_qualityOn)
        {
            QualitySettings.SetQualityLevel(2);
            _qualityImage.sprite = _qualityOnSprite;
            _postProcessVolume.enabled = true;
        }
        else
        {
            QualitySettings.SetQualityLevel(0);
            _qualityImage.sprite = _qualityOffSprite;
            _postProcessVolume.enabled = false; 
        }

        PlayerPrefs.SetInt("SavedQuality", _qualityOn ? 1 : 0);
    }

    private void LoadAudio(bool _enabled)
    {
        _audioListener.enabled = _enabled;
        if (_enabled)
            _audioImage.sprite = _audioOnSprite;
        else
            _audioImage.sprite = _audioOffSprite;
        PlayerPrefs.SetInt("SavedAudio", _enabled ? 1 : 0);
    }

    public void SwitchAudioSettings()
    {
        Debug.Log("Audio Button");
        _audioOn = !_audioOn;
        if (_audioOn)
        {
            _audioListener.enabled = true;
            _audioImage.sprite =_audioOnSprite;
        }
        else
        {
            _audioListener.enabled = false;
            _audioImage.sprite = _audioOffSprite;
        }

        PlayerPrefs.SetInt("SavedAudio", _audioOn ? 1 : 0);
    }

    public void LaunchURL(int _URLindex)
    {
        switch (_URLindex)
        {
            case 0:
                Application.OpenURL(_twitterURL);
                break;
            case 1:
                Application.OpenURL(_facebookURL);
                break;
        }
    }
}