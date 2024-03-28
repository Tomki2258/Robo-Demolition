using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelUpUI : MonoBehaviour
{
    public List<PowerUpClass> _powerUpsList;
    private int _powerUpsCount = 0;
    public PowerUpClass _leftPowerUp;
    public PowerUpClass _rightPowerUp;
    private void Awake()
    {
        _powerUpsList.Add(new PowerUpClass(PowerUpsEnum.Health,"Increase health by 10"));
        _powerUpsList.Add(new PowerUpClass(PowerUpsEnum.Damage,"Increase health by 10"));
        _powerUpsList.Add(new PowerUpClass(PowerUpsEnum.Range,"Increase health by 10"));
        _powerUpsList.Add(new PowerUpClass(PowerUpsEnum.Regeneration,"Increase health by 10"));
        _powerUpsList.Add(new PowerUpClass(PowerUpsEnum.ReloadSped,"Increase health by 10"));

        _powerUpsCount = _powerUpsList.Count;
    }

    public void ChooseReward(bool _isLeft)
    {
        // Dodaj wybór nagrody
        switch (_isLeft)
        {
            case true:
                break;
            case false:
                break;
        }
    }
    public void SetReward()
    {
        int _randomOne = Random.Range(0, _powerUpsCount);
        _leftPowerUp = _powerUpsList[_randomOne];
        
        int _randomTwo = Random.Range(0, _powerUpsCount);
        while (_randomTwo == _randomOne)
        {
            _randomTwo = Random.Range(0, _powerUpsCount);
        }
        _rightPowerUp = _powerUpsList[_randomTwo];
        Debug.LogWarning($"{_leftPowerUp.GetPowerUpType()} / {_rightPowerUp.GetPowerUpType()}");
        
        //ustawianie wartości do okienek oraz powrót z pauzy 
    }
}
