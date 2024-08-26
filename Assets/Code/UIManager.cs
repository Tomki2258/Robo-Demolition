using System;
using System.Collections;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    public GameObject _levelUpCanvas;

    [Header("Hp UI")] public Slider _hpSlider;

    [SerializeField] public TMP_Text _hpText;

    [Header("XP UI")] public TMP_Text _xpText;

    public TMP_Text _xPProgressText;
    public Slider _xpSlider;
    public GameObject _mainUI;
    public GameObject _gameStartUI;
    public GameObject _weaponUI;
    public GameObject _dieCanvas;
    public TMP_Text _timeText;
    public TMP_Text _killedEnemiesText;
    public GameObject _hpDifferenceText;
    public PlayerMovement _player;
    public GameObject _adRewardButton;
    public GameObject _fpsCanvas;
    public bool _enableFPS;
    public GameObject _settingsUI;
    public Image _captureAreaImage;
    public GameObject _eqCanvas;
    private GameManager _gameManager;
    private bool _startOverrided;
    public DateTime _startTime;
    private UserData _userData;
    private CameraShake _cameraShake;
    public GameObject _newEnemySpottedUI;
    [SerializeField] private GameObject _notyficationBaner;
    [SerializeField] private VariableJoystick _variableJoystick;
    public GameObject _questCanvas;
    private void Awake()
    {
        _newEnemySpottedUI.SetActive(false);
        _cameraShake = FindFirstObjectByType<CameraShake>();
        _userData = FindFirstObjectByType<UserData>();
        _eqCanvas.SetActive(false);
        _captureAreaImage.enabled = false;
        _settingsUI.SetActive(false);
        _levelUpCanvas.SetActive(false);
        QualitySettings.vSyncCount = 1;
        _gameManager = FindFirstObjectByType<GameManager>();
        _dieCanvas.SetActive(false);

        _startTime = DateTime.Now;

        /*
        if(_enableFPS) _fpsCanvas.SetActive(true);
        else _fpsCanvas.SetActive(false);
        */
    }

    private void Start()
    {
        if (!_startOverrided) StartGame(false);
        else StartGame(true);
    }

    public void ShowSpottedUI()
    {
        _newEnemySpottedUI.SetActive(true);
        StartCoroutine(DisableSpottedUI());
    }
    private IEnumerator DisableSpottedUI()
    {
        yield return new WaitForSeconds(2);
        _newEnemySpottedUI.SetActive(false);
    }
    public void Update()
    {
        //if(!_gameManager._gameLaunched) return;
        if (_levelUpCanvas.activeSelf)
        {
            //_mainUI.SetActive(false);
        }

        //_mainUI.SetActive(true);
        // if (_player._died)
        // {
        //     _mainUI.SetActive(false);
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         SceneManager.LoadScene(0);
        //     }
        // }
    }

    public void EnableEqCanvas(bool _mode)
    {
        _mainUI.SetActive(!_mode);
        Time.timeScale = _mode ? 0 : 1;
        _eqCanvas.SetActive(_mode);
        var _eqCanvasScript = FindFirstObjectByType<EquipmentCanvas>();
        if (_mode)
            _eqCanvasScript.CheckForWeaponPanels();
        else
        {
            _player._playerWeapons.SetWeaponsInUse();
            _notyficationBaner.SetActive(false);
        }
    }

    public void DoOverideStart()
    {
        _startOverrided = true;
    }

    public void ShowHpDifference(float _value)
    {
        var _hpDifference = Instantiate(_hpDifferenceText, _hpText.transform.position, Quaternion.identity);
        _hpDifference.transform.SetParent(_mainUI.transform);
        var _randomPosition = new Vector3(transform.position.x + Random.Range(100, 200),
            transform.position.y + Random.Range(100, 200),
            transform.position.z);
        _hpDifference.transform.position = _randomPosition;
        _hpDifference.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        var _text = _hpDifference.GetComponent<TMP_Text>();
        if (_value != -1 && _value != -2)
        {
        
            string _toDisplay = _value.ToString("0.00");
            _text.text = _toDisplay.Replace(",", ".");

            if (_value > 0)
                _text.color = Color.green;
            else
                _text.color = Color.red;
        }
        else if(_value == -1)
        {
            Debug.LogWarning("BULLET MISS");
            _text.text = "Miss";
            _text.color = Color.white;
        }
        else if (_value == -2)
        {
            _text.text = "SHIELD";
            _text.color = Color.white;
        }
        Destroy(_hpDifference, 1.1f);
    }

    public void UnlockUI(string _weaponTypeText, Sprite _weaponSprite)
    {
        _weaponUI.SetActive(true);
        var _weaponImage = _weaponUI.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        var _weaponText = _weaponUI.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();

        _weaponText.text = _weaponTypeText;
        StartCoroutine(DisableObject(_weaponUI));
    }

    private IEnumerator DisableObject(GameObject _object)
    {
        yield return new WaitForSeconds(2);
        _object.SetActive(false);
    }

    public void DoLevelUpCanvas(bool _sraka)
    {
        //_variableJoystick.gameObject.SetActive(!_sraka);
        _mainUI.SetActive(!_sraka);
        _levelUpCanvas.SetActive(_sraka);
        _levelUpCanvas.SetActive(_sraka);

        if (_sraka)
            _levelUpCanvas.GetComponent<LevelUpUI>().SetReward();
        if (!_sraka)
        {
            _cameraShake.SwitchShakeMode(true);
            _player.DoJoystickInput(true);
        }
        Time.timeScale = !_sraka ? 1 : 0;
    }

    public void EnableDieCanvas(bool _mode)
    {
        if(_mode)
        {
            FindFirstObjectByType<RewardAd>().LoadAd();
            _dieCanvas.SetActive(true);
            _mainUI.SetActive(false);
            _player.DoJoystickInput(false);
            var _time = DateTime.Now - _startTime;
            if (_gameManager._killedEnemies > _userData.GetBestScore())
            {
                int _killedEnemies = _gameManager._killedEnemies;
                _killedEnemiesText.text = $"New high score !\nKilled enemies: {_gameManager._killedEnemies}";
                _userData.SaveBestScore(_killedEnemies);
            }
            else
            {
                _killedEnemiesText.text = $"Killed enemies: {_gameManager._killedEnemies}\n" +
                                          $"Best score: {_userData.GetBestScore()}";
            }

            var formattedTime = $"{_time.Minutes:D2} minutes:{_time.Seconds:D2} seconds";
            _timeText.text = $"Playtime: {formattedTime}";
            /*
            if (_adRewardButton.GetComponent<Button>().interactable == false)
            {
                _adRewardButton.SetActive(false);
            }
            */
            _mainUI.SetActive(false);
        }
        else
        {
            _dieCanvas.SetActive(false);
            _mainUI.SetActive(true);
        }
    }

    private int GetBestScore()
    {
        var _best = PlayerPrefs.GetInt("BestScore");
        return _best;
    }

    public void ReloadLevel()
    {
        StartCoroutine(_gameManager.ReloadLevel());
    }

    public void RetryAdRelaod()
    {
        FindFirstObjectByType<RewardAd>().ShowAd();
    } 

    public void StartGame(bool state)
    {
        //Debug.LogWarning("to sie robi ?");
        _gameStartUI.SetActive(!state);
        _mainUI.SetActive(state);
    }

    public void EnableSettingsUI(bool _mode)
    {
        _player.DoJoystickInput(!_mode);
        _mainUI.SetActive(!_mode);
        _settingsUI.SetActive(_mode);
        if (_mode) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    public void EnableQuestUI(bool _mode)
    {
        _questCanvas.SetActive(_mode);
    }
}