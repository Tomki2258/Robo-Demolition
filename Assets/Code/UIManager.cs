using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private void Awake()
    {
        _player = FindAnyObjectByType<PlayerMovement>();
        _levelUpCanvas.SetActive(false);
        QualitySettings.vSyncCount = 1;
        _gameManager = FindFirstObjectByType<GameManager>();
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
}