using System.Collections.Generic;
using UnityEngine;

public class CaptureAreaManager : MonoBehaviour
{
    [Header("Capture areas")] public GameObject _currentCaptureArea;
    private GameManager _gameManager;
    public GameObject _captureAreaPrefab;
    public float _currentCaptureWaitTime;
    private List<Transform> _zonesRespawns;
    public int _maxCaptureWaitTime;
    private PlayerMovement _player;
    void Start()
    {
        _gameManager = GetComponent<GameManager>();
        _player= _gameManager._player;
        _zonesRespawns = _gameManager._spawnPoints;
    }

    void FixedUpdate()
    {
        if(_gameManager._gameLaunched)
            DoCaptureAreas();
    }
    private void DoCaptureAreas()
    {
        if (_currentCaptureArea != null) return;
        
        if (_currentCaptureWaitTime > _maxCaptureWaitTime)
        {
            var _randomSpawn = Random.Range(0, _zonesRespawns.Count);
            var _targetVector = new Vector3(_zonesRespawns[_randomSpawn].transform.position.x,
                _zonesRespawns[_randomSpawn].transform.position.y + 0.5f,
                _zonesRespawns[_randomSpawn].transform.position.z-10);

            _currentCaptureArea = Instantiate(_captureAreaPrefab, _targetVector, Quaternion.identity);
        }
        else
        {
            _currentCaptureWaitTime += Time.deltaTime;
        }
    }
    public void GetCaptureAreaReward()
    {
        _player._health = _player._maxHealth;
        _player.AddPlayerXP(_player._xpToNextLevel - _player._xp);
        _gameManager._notyficationBaner.ShotMessage("ZONE CAPTURED", 
            "Player boosted",false,true);
    }
}
