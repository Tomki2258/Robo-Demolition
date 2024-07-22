using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    private int _allKilldedEnemies;
    private int _playerCoins;
    public bool _IsOnline;
    private void Start()
    {
        _allKilldedEnemies = PlayerPrefs.GetInt("allKilledEnemies");
        _playerCoins = PlayerPrefs.GetInt("allCoins");
    }

    public int GetKilledEnemies()
    {
        return _allKilldedEnemies;
    }

    public int GetPlayerCoins()
    {
        return _playerCoins;
    }

    public void AddPlayerCoins(int coins)
    {
        int _allCoins = this.GetPlayerCoins() + coins;
        PlayerPrefs.SetInt("allCoins",_allCoins);
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
}
