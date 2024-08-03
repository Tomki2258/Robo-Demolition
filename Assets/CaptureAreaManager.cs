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
    void Start()
    {
        _gameManager = GetComponent<GameManager>();
        _zonesRespawns = _gameManager._spawnPoints;
    }

    void FixedUpdate()
    {
        DoCaptureAreas();
    }
    private void DoCaptureAreas()
    {
        if (_currentCaptureArea == null)
        {
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
    }
}
