using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject _levelUpCanvas;
    
    [Header("Hp UI")]
    public Slider _hpSlider;
    [SerializeField] public TMP_Text _hpText;
    private PlayerMovement _player;
    [Header("XP UI")] 
    public TMP_Text _xpText;

    public TMP_Text _xPProgressText;
    public Slider _xpSlider;
    private void Awake()
    {
        _player = FindAnyObjectByType<PlayerMovement>();
        _levelUpCanvas.SetActive(false);
    }

    private void FixedUpdate()
    {
        _hpText.text = $"{Math.Round(_player._health)}" +
                       $"/" +
                       $"{_player._maxHealth}";
        _hpSlider.value = _player._health;
        _hpSlider.maxValue = _player._maxHealth;
        
        _xpText.text = $"{_player._level}";
        
        _xpSlider.value = _player._xp;
        _xpSlider.maxValue = _player._xpToNextLevel;
        _xPProgressText.text = $"{_player._xp}" +
                              $"/" +
                              $"{_player._xpToNextLevel}";
    }

    public void DoLevelUpCanvas()
    {
        _levelUpCanvas.SetActive(true);
    }
}