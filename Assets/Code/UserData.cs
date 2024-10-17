using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    private int _allKilldedEnemies;
    [SerializeField] private int _playerCoins;
    [SerializeField] private int _deathCount;
    public bool _IsOnline;
    private void Start()
    {
        _allKilldedEnemies = PlayerPrefs.GetInt("allKilledEnemies");
        _playerCoins = PlayerPrefs.GetInt("allCoins");
    }

    private int GetKilledEnemies()
    {
        return _allKilldedEnemies;
    }

    public int GetPlayerCoins()
    {
        return _playerCoins;
    }

    public void AddPlayerCoins(int coins)
    {
        _playerCoins += coins;
        PlayerPrefs.SetInt("allCoins",_playerCoins);
    }
    public void AddKilledEnemies(int killedEnemies)
    {
        int _allKilled = this.GetKilledEnemies() + killedEnemies;
        PlayerPrefs.SetInt("allKilledEnemies",_allKilled);
    }

    public void SaveBestScore(int _bestScore)
    {
        PlayerPrefs.SetInt("BestScore",_bestScore);        
    }

    public int GetBestScore()
    {
        return PlayerPrefs.GetInt("BestScore");
    }
    public void CheckPlayerOnline(){
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            _IsOnline = false;
            return;
        }
        _IsOnline = true;
    }
    public void AddDeathCount()
    {
        _deathCount++;
        PlayerPrefs.SetInt("DeathCount",_deathCount);
    }
    public int GetDeathCount()
    {
        return PlayerPrefs.GetInt("DeathCount");
    }
}
