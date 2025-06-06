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
    public bool _speededUpGame;
    public List<Transform> _spawnPoints;
    public PlayerMovement _player;
    public float _spawnOffset;
    
    [Header("------------------")]
    public GameObject _pausedUI;
    public float _spawnTimeCurrent;
    public int _killedEnemies;
    public GameSettings _gameSettings;
    public GameObject _explosion;
    public GameObject _spodek;
    public GameObject _continueCanvas;
    
    public WeaponUnlock _weaponUnlock;
    public NotyficationBaner _notyficationBaner;
    private GameObject[] _bombSpawns;
    private CameraController _cameraController;
    private int _enemiesCount;
    private InterstitialAd _interstitialAd;
    private bool _paused;
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
    [SerializeField] private List<GameObject> _disableBuildObjects;
    [SerializeField] private List<Transform> _playerPossibleSpawners;
    [SerializeField] private ParticleSystem _spawnParticles;
    private Transform _choosenSpawner;
    [SerializeField] private int _poweredEnemyChance;
    private bool _playerWasRevived = false;
    [SerializeField] private Camera _garageCammera;
    [SerializeField] private GameObject _garageSpace;
    [SerializeField] private Camera _mainCamera;
    public GameObject _poweredEnemyEffect;
    [SerializeField] private GameObject _mainLight;
    public EnemyFactory _enemyFactory;
    private void Awake()
    {
        _enemyFactory = GetComponent<EnemyFactory>();
        _garageCammera.enabled = false;
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
        _enemiesCount = _enemyFactory._enemies.Count;
        foreach (var _obj in _spawnPoints) _obj.name = "Enemy Spawn Point";
        _pausedUI.SetActive(false);
        if (!_gameLaunched) _player.DoJoystickInput(false);
        _interstitialAd = FindFirstObjectByType<InterstitialAd>();
        _gameSettings = FindFirstObjectByType<GameSettings>();
        _uiManager = FindFirstObjectByType<UIManager>();
        _player.gameObject.SetActive(false);
        _spawnTimeCurrent = _enemyFactory._spawnTimeMax;
        if(_godMode) DoGodMode();
        if (_gameStarted && _gameLaunched) OverideStart();
        
        _choosenSpawner = _playerPossibleSpawners[Random.Range(0,
            _playerPossibleSpawners.Count)];
        Vector3 _spawnVector = new Vector3(
            _choosenSpawner.position.x,
            _choosenSpawner.position.y + 110,
            _choosenSpawner.position.z);
        _spodek.transform.position = _spawnVector;

        if (_speededUpGame)
        {
            Time.timeScale = 3;
        }
    }

    private void DoAppLaunch()
    {
        if (!Application.isEditor)
        {
            _godMode = false;
            _gameLaunched = false;
            _gameStarted = false;
            _speededUpGame = false;
            foreach (GameObject _gameObject in _disableBuildObjects) 
            {
                _gameObject.SetActive(false);
            }
        }
    }
    public void DoGodMode()
    {
        _player._maxHealth = 10000;
        _player._level = 100;
        _player._xpToNextLevel = 10000;
        _enemyFactory._possibleEnemies = _enemiesCount;
        _enemyFactory._spawnTimeMax *= 0.5f;
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        if (_spawnTimeCurrent < _enemyFactory._spawnTimeMax)
        {
            _spawnTimeCurrent += Time.deltaTime;
            return;
        }

        _enemyFactory.SpawnEnemy();
        _spawnTimeCurrent = 0;
    }

    private void DestroyTrash()
    {
        GameObject[] _trashArray;
        _trashArray = GameObject.FindGameObjectsWithTag("Trash");
        foreach (var _trash in _trashArray) Destroy(_trash);
    }
    
    public void IncreaseEnemiesIndex()
    {
        if (!_enemyFactory._enemiesStages.Contains(_player._level)) return;
        
        _enemyFactory._possibleEnemies++;
        _uiManager.ShowSpottedUI();
    }
    
    public void RemoveEnemy(GameObject _enemy)
    {
        _enemyFactory._spawnedEnemies.Remove(_enemy);
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
        
        _uiManager._dieCanvas.SetActive(false);
        _uiManager._mainUI.SetActive(true);

        _player.Revive();
        EnableContinueCanvas(true);
        
        foreach (var _enemy in _enemyFactory._spawnedEnemies.ToList())
        {
            Enemy _enemyScript = _enemy.GetComponent<Enemy>();
            _enemyScript._killedByManager = true;
            _enemyScript.CheckHealth(_enemyScript._health + 1);
        }

        _enemyFactory._spawnedEnemies.Clear();
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
        _uiManager._questCanvas.SetActive(false);
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
                Instantiate(_startGameParticle, _choosenSpawner.position, Quaternion.identity);
            ParticleSystem _particleSystem = _startParticleInstance.GetComponent<ParticleSystem>();
            _particleSystem.Play();
            //Destroy(_startParticleInstance,10);
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

    private IEnumerator StartDelayCoroutine(float delay){
		yield return new WaitForSeconds(delay);
		StartGame();
	}

	public void StartDelay(float delay){
		StartCoroutine(StartDelayCoroutine(delay));
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

    public void EnablePlayerGarage(bool _mode)
    {
        _garageCammera.enabled = _mode;
        _garageSpace.SetActive(_mode);
        _mainCamera.enabled = !_mode;
        //_mainLight.SetActive(!_mode);
        //_uiManager.EnablePlayerCustomizationCanvas(_mode);

        RenderSettings.fog = _mode ? true : false;
    }

    public float GetTrashX()
    {
        return Random.Range(-_trashRotateX, _trashRotateX);
    }
    public float GetTrashY()
    {
        return Random.Range(-_trashRotateY, _trashRotateY);
    }

    public int GetPoweredEnemyChance()
    {
        return _poweredEnemyChance;
    }

    public void SetPlayerRevived()
    {
        _playerWasRevived = true;
    }
    public bool GetPlayerRevived()
    {
        return _playerWasRevived;
    }
}