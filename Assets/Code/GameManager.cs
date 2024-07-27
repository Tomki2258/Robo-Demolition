using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public bool _godMode;
    public bool _gameStarted;
    public bool _gameLaunched;
    public List<Transform> _spawnPoints;
    public float _spawnTimeMax;
    public PlayerMovement _player;
    public float _spawnOffset;
    [Header("Enemies Section")]
    public List<GameObject> _enemies;
    public List<int> _enemiesStages;
    public int _possibleEnemies;
    public List<GameObject> _spawnedEnemies;
    [Header("------------------")]
    public float _powerUpSpawnTimeMax;
    public List<Transform> _powerUpSpawnPoints;
    public GameObject _powerUps;
    public GameObject _pausedUI;
    public float _spawnTimeCurrent;
    public int _killedEnemies;
    public GameSettings _gameSettings;
    public GameObject _explosion;
    public GameObject _spodek;
    public GameObject _continueCanvas;

    [Header("Capture areas")] public GameObject _currentCaptureArea;

    public GameObject _captureAreaPrefab;
    public float _currentCaptureWaitTime;
    public int _maxCaptureWaitTime;
    public WeaponUnlock _weaponUnlock;
    public NotyficationBaner _notyficationBaner;
    private GameObject[] _bombSpawns;
    private CameraController _cameraController;
    private int _enemiesCount;
    private InterstitialAd _interstitialAd;
    private bool _paused;
    private float _powerUpSpawnTimeCurrent;
    private List<GameObject> _spawnedPowerUps;
    private int _spawnsCount;
    private UIManager _uiManager;
    public AudioSource _spodekAudioSource;
    [SerializeField] [Range(0, 120)] private int _trashRotateX;
    [SerializeField] [Range(0, 120)] private int _trashRotateY;
    public Material _hitMaterial;
    public Material _blackMaterial;
    public GameObject _secondSpodek;
    private UserData _userData;
    public GameObject _startGameParticle;
    [SerializeField] private AnimationClip _animationClip;
    [SerializeField] private Animator _pauseAnimator;
    private bool _unPauseStarted = false;
    [SerializeField] private AudioListener _mainAudioListener;
    private void Awake()
    {
        DoAppLaunch();
        _userData = FindFirstObjectByType<UserData>();
        _userData.CheckPlayerOnline();
        _notyficationBaner = FindFirstObjectByType<NotyficationBaner>();
        _notyficationBaner._notyficationBaner.SetActive(false);
        _bombSpawns = GameObject.FindGameObjectsWithTag("bombersSpawner");
        _weaponUnlock.PrepareWeapons();
        _player = FindAnyObjectByType<PlayerMovement>();
        _cameraController = FindFirstObjectByType<CameraController>();

        _player.transform.GetComponent<AudioListener>().enabled = false;
        _cameraController.gameObject.GetComponent<AudioListener>().enabled = true;
        DestroyTrash();
        _spodek.SetActive(false);
        _spawnsCount = _spawnPoints.Count;
        _enemiesCount = _enemies.Count;
        foreach (var _obj in _spawnPoints) _obj.name = "Enemy Spawn Point";
        _pausedUI.SetActive(false);
        if (!_gameLaunched) _player.DoJoystickInput(false);
        _interstitialAd = FindFirstObjectByType<InterstitialAd>();
        _gameSettings = FindFirstObjectByType<GameSettings>();
        _uiManager = FindFirstObjectByType<UIManager>();
        _player.gameObject.SetActive(false);
        _spawnTimeCurrent = _spawnTimeMax;
        if(_godMode) DoGodMode();
        if (_gameStarted && _gameLaunched) OverideStart();
    }

    private void DoAppLaunch()
    {
        if (!Application.isEditor)
        {
            _godMode = false;
            _gameLaunched = false;
            _gameStarted = false;
        }
    }
    public void DoGodMode()
    {
        _player._maxHealth = 10000;
        _player._level = 100;
        _player._xpToNextLevel = 10000;
        _possibleEnemies = _enemiesCount;
        _godMode = true;
    }
    private void FixedUpdate()
    {
        if (!_gameLaunched || _player._died)
        {
            Debug.Log("Player died");
            return;
        }
        //if(!_player._died) return;

        DoCaptureAreas();

        if (_spawnTimeCurrent < _spawnTimeMax)
        {
            _spawnTimeCurrent += Time.deltaTime;
            return;
        }

        SpawnEnemy();
        _spawnTimeCurrent = 0;
    }

    private void DestroyTrash()
    {
        GameObject[] _trashArray;
        _trashArray = GameObject.FindGameObjectsWithTag("Trash");
        foreach (var _trash in _trashArray) Destroy(_trash);
    }

    private void DoCaptureAreas()
    {
        if (_currentCaptureArea == null)
        {
            if (_currentCaptureWaitTime > _maxCaptureWaitTime)
            {
                var _randomSpawn = Random.Range(0, _bombSpawns.Length);
                var _targetVector = new Vector3(_bombSpawns[_randomSpawn].transform.position.x,
                    _bombSpawns[_randomSpawn].transform.position.y + 0.5f,
                    _bombSpawns[_randomSpawn].transform.position.z-10);

                _currentCaptureArea = Instantiate(_captureAreaPrefab, _targetVector, Quaternion.identity);
            }
            else
            {
                _currentCaptureWaitTime += Time.deltaTime;
            }
        }
    }
    public void ShowNewEnemySpotted()
    {
    }
    public void IncreaseEnemiesIndex()
    {
        if (_enemiesStages.Contains(_player._level))
        {
            _possibleEnemies++;
            _uiManager.ShowSpottedUI();
        }
    }

    private void SpawnEnemy()
    {
        var _point = Random.Range(0, _spawnsCount);
        var _distance = Vector3.Distance(_spawnPoints[_point].position, _player.transform.position);
        if (_distance < _spawnOffset) 
            _point = Random.Range(0, _spawnsCount);

        // Spawn enemy
        var _enemyIndex = Random.Range(0, _possibleEnemies);
        var _randomSpawnVector = new Vector3(_spawnPoints[_point].position.x + Random.Range(-3, 3),
            _spawnPoints[_point].position.y,
            _spawnPoints[_point].position.z + Random.Range(-3, 3));
        var _enemy = Instantiate(_enemies[_enemyIndex], _randomSpawnVector, Quaternion.identity);
        _spawnTimeMax -= _spawnTimeMax * 0.005f;

        if (_enemy.GetComponent<BombardEnemy>()) return;

        _spawnedEnemies.Add(_enemy);
        _enemy.GetComponent<Enemy>()._gameManager = this;
        _enemy.GetComponent<Enemy>()._player = _player;
    }

    private void SpawnPowerUp()
    {
        var _point = Random.Range(0, _spawnsCount);
        var _distance = Vector3.Distance(_powerUpSpawnPoints[_point].position, _player.transform.position);
        if (_distance < _spawnOffset) return;

        //var _powerUpIndex = Random.Range(0, _enemiesCount);
        var _currentPowerUp = Instantiate(_powerUps, _powerUpSpawnPoints[_point].position, Quaternion.identity);
        _spawnedPowerUps.Add(_currentPowerUp);
    }

    public void RemoveEnemy(GameObject _enemy)
    {
        _spawnedEnemies.Remove(_enemy);
    }

    public void EnableContinueCanvas(bool _mode)
    {
        Time.timeScale = _mode ? 0 : 1;
        _continueCanvas.SetActive(_mode);
        _pausedUI.SetActive(false);
        _uiManager._mainUI.SetActive(!_mode);
    }

    public void PauseGame()
    {
        if(_unPauseStarted) return;
        
        _paused = !_paused;
        if (_paused)
        {
            AudioListener.pause = true;
            _player.DoJoystickInput(false);
            Time.timeScale = 0;
            _pausedUI.SetActive(true);
            _uiManager._mainUI.SetActive(false);
        }
        else
        {
            _unPauseStarted = true;
            StartCoroutine(unPauseIEnumerator());
        }
    }

    private IEnumerator unPauseIEnumerator()
    {
        _pauseAnimator.SetTrigger("isOpen");
        yield return new WaitForSecondsRealtime(_animationClip.length);
        AudioListener.pause = false;
        _player.DoJoystickInput(true);
        Time.timeScale = 1;
        _pausedUI.SetActive(false);
        _uiManager._mainUI.SetActive(true);
        _unPauseStarted = false;
    }
    public void DoAd()
    {
        var _random = Random.Range(0, 3);
        if (_random == 0) _interstitialAd.ShowAd();
    }

    public void AdReward()
    {
        Debug.LogWarning("Ad reward");
        foreach (var _enemy in _spawnedEnemies)
        {
            _enemy.GetComponent<Enemy>()._killedByManager = true;
            Destroy(_enemy);
        }

        _spawnedEnemies.Clear();

        _uiManager._dieCanvas.SetActive(false);
        _uiManager._mainUI.SetActive(true);

        _player.Revive();
        EnableContinueCanvas(true);
    }

    public IEnumerator ReloadLevel()
    {
        _userData.AddKilledEnemies(_killedEnemies);
        _uiManager._dieCanvas.SetActive(false);
        var _explosionn = Instantiate(_explosion, _player.transform.position, Quaternion.identity);
        _player.DiePlayerTexture();
        yield return new WaitForSeconds(3);
        Destroy(_explosionn);
        SceneManager.LoadScene(SceneManager.loadedSceneCount);
    }
    

    public void StartGame()
    {
        _spodek.SetActive(true);
        _uiManager._gameStartUI.SetActive(false);
        _gameStarted = true;
        _spodekAudioSource.Play();
        StartCoroutine(MakeGame());
    }
    private IEnumerator MakeGame()
    {
        if(_godMode) DoGodMode();
        
        yield return new WaitForSeconds(4.2f);
        if (_startGameParticle != null)
        {
            Debug.LogWarning("Start particle");
            GameObject _startParticleInstance =
                Instantiate(_startGameParticle, _spodek.transform.position, Quaternion.identity);
            ParticleSystem _particleSystem = _startParticleInstance.GetComponent<ParticleSystem>();
            _particleSystem.Play();
            Destroy(_startParticleInstance,10);
        }
        yield return new WaitForSeconds(0.8f);
        _secondSpodek.SetActive(false);
        _player.transform.GetComponent<AudioListener>().enabled = true;
        _cameraController.gameObject.GetComponent<AudioListener>().enabled = false;

        _player.transform.position = _spodek.transform.position;
        _gameLaunched = true;
        _cameraController.SwitchTarget(_player.transform);
        _player.DoJoystickInput(true);
        _uiManager.StartGame(true);
        _player.gameObject.SetActive(true);
        //Debug.LogWarning("Game made lol");
        _spodek.SetActive(false);
    }
	public IEnumerator StartDelayCoroutine(float delay){
		yield return new WaitForSeconds(delay);
		StartGame();
	}

	public void StartDelay(float delay){
		StartCoroutine(StartDelayCoroutine(delay));
	}

    public void GetCaptureAreaReward()
    {
        _player._health = _player._maxHealth;
        _player._xp += _player._xpToNextLevel / 2;
    }

    private void OverideStart()
    {
        Debug.LogWarning("Start Overided!");
        _uiManager.DoOverideStart();
        _player.gameObject.SetActive(true);
        _player.enabled = true;
        _spodek.SetActive(false);
        _gameStarted = true;
        _gameLaunched = true;
        _player.transform.GetComponent<AudioListener>().enabled = true;
        _cameraController.gameObject.GetComponent<AudioListener>().enabled = false;
    }

    public float GetTrashX()
    {
        return Random.Range(-_trashRotateX, _trashRotateX);
    }
    public float GetTrashY()
    {
        return Random.Range(-_trashRotateY, _trashRotateY);
    }
}