using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private PlayerMovement _player;
    public GameObject _mainUI;
    private GameManager _gameManager;
    public GameObject _weaponUI;
    public DateTime _startTime;
    public GameObject _dieCanvas;
    public TMP_Text _timeText;
    public TMP_Text _killedEnemiesText;
    public GameObject _hpDifferenceText;
    private void Awake()
    {
        _player = FindAnyObjectByType<PlayerMovement>();
        _levelUpCanvas.SetActive(false);
        QualitySettings.vSyncCount = 1;
        _gameManager = FindFirstObjectByType<GameManager>();
        
        _startTime = DateTime.Now;
    }

    public void ShowHpDifference(float _value)
    {
        GameObject _hpDifference = Instantiate(_hpDifferenceText, _hpText.transform.position, Quaternion.identity);
        _hpDifference.transform.SetParent(_mainUI.transform);
        Vector3 _randomPosition = new Vector3(transform.position.x + Random.Range(-50,50),
            transform.position.y + Random.Range(-50,50),
            transform.position.z);
        _hpDifference.transform.position = _randomPosition;
        _hpDifference.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        TMP_Text _text = _hpDifference.GetComponent<TMP_Text>();
        _text.text = _value.ToString();

        if (_value > 0)
        {
            _text.color = Color.green;
        }
        else
        {
            _text.color = Color.red;
        }
        Destroy(_hpDifference, 1.1f);
    }
    private void FixedUpdate()
    {
        if(!_gameManager._gameLaunched) return;
        _hpText.text = $"{Math.Round(_player._health)}" +
                       "/" +
                       $"{_player._maxHealth}";
        _hpSlider.value = _player._health;
        _hpSlider.maxValue = _player._maxHealth;

        _xpText.text = $"{_player._level}";

        _xpSlider.value = _player._xp;
        _xpSlider.maxValue = _player._xpToNextLevel;
        _xPProgressText.text = $"{_player._xp}" +
                               "/" +
                               $"{_player._xpToNextLevel}";

        if (_levelUpCanvas.activeSelf)
        {
            _mainUI.SetActive(false);
        }
        else
        {
            _mainUI.SetActive(true);
        }

        if (_player._died)
        {
            _mainUI.SetActive(false);
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void UnlockUI(String _weaponTypeText,Sprite _weaponSprite)
    {
        _weaponUI.SetActive(true);
        Image _weaponImage = _weaponUI.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        TMP_Text _weaponText = _weaponUI.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();

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
        _levelUpCanvas.SetActive(_sraka);
        if(_sraka)
            _levelUpCanvas.GetComponent<LevelUpUI>().SetReward();
        if(!_sraka)
            _player.DoJoystickInput(true);
        Time.timeScale = !_sraka ? 1 : 0;
    }

    public void EnableDieCanvas()
    {
        _dieCanvas.SetActive(true);
        _mainUI.SetActive(false);
        TimeSpan _time = DateTime.Now - _startTime;
        if(_gameManager._killedEnemies > GetBestScore())
        {
            PlayerPrefs.SetInt("BestScore", _gameManager._killedEnemies);
            _killedEnemiesText.text = $"New high score !\nKilled enemies: {_gameManager._killedEnemies}";
        }
        else
        {
            _killedEnemiesText.text = $"Killed enemies: {_gameManager._killedEnemies}\n" +
                                      $"Best score: {GetBestScore()}";
        }
        _timeText.text = $"Time: {_time.Minutes}:{_time.Seconds}";
    }

    private int GetBestScore()
    {
        int _best = PlayerPrefs.GetInt("BestScore");
        return _best;
    }
}