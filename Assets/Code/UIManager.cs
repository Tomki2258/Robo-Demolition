using System;
using System.Collections;
using System.IO;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    public GameObject _levelUpCanvas;

    [Header("Hp UI")] public Slider _hpSlider;
    [SerializeField] private Transform _hpNotyficationReference;
    private Vector3 _hpNotyficationPosition;

    [SerializeField] public TMP_Text _hpText;

    [Header("XP UI")] public TMP_Text _xpText;

    public TMP_Text _xPProgressText;
    public Slider _xpSlider;
	public GameObject _playerHpUi;
    public GameObject _playerXpUi;
    public GameObject _mainUI;
    public GameObject _gameStartUI;
    public GameObject _weaponUI;
    public GameObject _dieCanvas;
    public TMP_Text _timeText;
    public TMP_Text _killedEnemiesText;
    public GameObject _hpDifferenceText;
    public PlayerMovement _player;
    public GameObject _settingsUI;
    public Image _captureAreaImage;
    public GameObject _eqCanvas;
    private GameManager _gameManager;
    private bool _startOverrided;
    public DateTime _startTime;
    public UserData _userData;
    private CameraShake _cameraShake;
    public GameObject _newEnemySpottedUI;
    [SerializeField] private GameObject _notyficationBaner;
    [SerializeField] private VariableJoystick _variableJoystick;
    public GameObject _questCanvas;
    private QuestManager _questManager;
    [SerializeField] private TMP_Text _coinsText;
    public GameObject _shieldButton;
    public Slider _shieldSlider;
    [Header("Player customization")]
    [SerializeField] private GameObject _playerCustomizationCanvas;
    private PlayerGarage _playerGarage;
    public TMP_Text _skinNameText;
    public TMP_Text _skinPriceText;
    public GameObject _lockedSkinBaner;
    public GameObject _selectSkinButton;
    public GameObject _buySkinButton;
    public TMP_Text _garageCoins;
    public GameObject _eqSkinInfo;
    [Header("CreditsCanvas")]
    public GameObject _creditsObject;
    public TMP_Text _creditsText;
    [Header("RateApp")] public GameObject _rateAppCanvas;
    private AppReview _appReview;
    private void Awake()
    {
        _questCanvas.SetActive(false);
        _questManager = FindFirstObjectByType<QuestManager>();
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
        _playerGarage = FindFirstObjectByType<PlayerGarage>();
        _startTime = DateTime.Now;
        _appReview = GetComponent<AppReview>();
        _rateAppCanvas.SetActive(false);
        _gameStartUI.SetActive(true);
        _playerCustomizationCanvas.SetActive(false);
        _shieldSlider.maxValue = _player._shieldMaxTimer;
        _shieldSlider.gameObject.SetActive(false);
        ManageShieldButton();
    }

    private void Start()
    {
        StartGame(_startOverrided);
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

    public void ShowXPDifference(int _value,bool _isLucky)
    {
        var _xpDifference = Instantiate(_hpDifferenceText, _xPProgressText.transform.position, Quaternion.identity);
        var _xpDiffTransform = _xpDifference.transform;
        _xpDiffTransform.SetParent(_playerXpUi.transform);
        var newPosition = new Vector3(_xpDifference.transform.position.x, _xpDifference.transform.position.y + Random.Range(100,200), _xpDifference.transform.position.z); //przesuwam lekko w prawo
        _xpDifference.transform.localScale = new Vector3(1.25f,1.25f,1.25f);
        _xpDifference.transform.position = newPosition;
        
        var _text = _xpDifference.GetComponent<TMP_Text>();
        
        _text.color = _isLucky ? Color.yellow : Color.cyan;

        
        string _valString = _value.ToString();
        _text.text = _isLucky ? "Lucky " + _valString : _valString;
        
        Destroy(_xpDifference, 1.1f);

    }
    public void ShowHpDifference(float _value)
    {
        var _hpDifference = Instantiate(_hpDifferenceText, _hpText.transform.position, Quaternion.identity);
        var _hpDiffTransform = _hpDifference.transform;
        _hpDiffTransform.SetParent(_playerHpUi.transform);
        var _randomPosition = new Vector3(_hpDifference.transform.position.x + Random.Range(350, 400), //tutaj tez
            _hpDifference.transform.position.y + Random.Range(100, 200), //100, 200
            _hpDifference.transform.position.z);
        _hpDiffTransform.position = _randomPosition;
        _hpDiffTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        var _text = _hpDifference.GetComponent<TMP_Text>();
        if (_value != -1 && _value != -2)
        {
            //-1 is for bullet miss -2 is fot shield
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

    public void DoLevelUpCanvas(bool _mode)
    {
        _mainUI.SetActive(!_mode);
        _levelUpCanvas.SetActive(_mode);
        _levelUpCanvas.SetActive(_mode);

        if (_mode)
            _levelUpCanvas.GetComponent<LevelUpUI>().SetReward();
        if (!_mode)
        {
            _cameraShake.SwitchShakeMode(true);
            _player.DoJoystickInput(true);
        }
        Time.timeScale = !_mode ? 1 : 0;
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
        QuestsMonitor _questsMonitor = FindFirstObjectByType<QuestsMonitor>();
        _questsMonitor.ApplyStats();
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

    public void EnableQuestUI()
    {
        bool _questEnabled = _questCanvas.activeSelf;
        
        _coinsText.text = _userData.GetPlayerCoins().ToString();
        
        if (_questCanvas)
        {
            _questCanvas.SetActive(!_questEnabled);
        }
        else
        {
            _questCanvas.SetActive(_questEnabled);
            //_questManager.CheckQuests();
        }
    }

    private String GetCreditsFromFile()
    {
        return Resources.Load<TextAsset>("credits").text;;
    }

    public void CloseReview()
    {
        _rateAppCanvas.SetActive(false);
        int _canceledRate = PlayerPrefs.GetInt("CanceledRate");
        PlayerPrefs.SetInt("CanceledRate",_canceledRate + 1);
    }
    public void EnablePlayerCustomizationCanvas(bool _mode)
    {
        _gameManager.EnablePlayerGarage(_mode);
        _playerCustomizationCanvas.SetActive(_mode);
        _playerGarage._garageEnabled = _mode;
        _gameStartUI.SetActive(!_mode);
        if (_mode)
        {
            _garageCoins.text = _userData.GetPlayerCoins().ToString();
        }
    }
    public void EnableCreditsUI(bool _mode)
    {
        _creditsObject.SetActive(_mode);
        _creditsText.text = GetCreditsFromFile();
    }
    public void ManageShieldButton()
    {
        _shieldButton.SetActive(_player._shieldCount > 0);
    }
}