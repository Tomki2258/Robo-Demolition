using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Hp UI")]
    public Slider _hpSlider;
    [SerializeField] public TMP_Text _hpText;
    private PlayerMovement _player;
    [Header("XP UI")] 
    public TMP_Text _xpText;
    private void Start()
    {
        _player = FindAnyObjectByType<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        _hpText.text = $"{Math.Round(_player._health)}" +
                       $"/" +
                       $"{_player._maxHealth}";
        _hpSlider.value = _player._health;
        _hpSlider.maxValue = _player._maxHealth;
        
        _xpText.text = $"{_player._level}";
    }
}