using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemyType
{
    Ground,
    Air
}
public class Enemy : MonoBehaviour
{
    public EnemyType _enemyType;
    public NavMeshAgent _agent;
    private GameManager _gameManager;
    public PlayerMovement _player;
    public GameObject _explosionPrefab;
    public int _attackRange;
    public int _stoppingDistance;
    public int _xpReward;
    public GameObject _bullet;
    public int _bulletDamage;
    public float _health;
    public float _maxHealth;
    public float _attackDelayMax;
    public float _attackDelayCurrent;
    public bool _stunned;
    public Material _hitMaterial;
    public bool _killedByManager;
    public Material _blackMaterial;
    private readonly int _maxStunTime = 5;
    private CameraShake _cameraShake;
    private GameObject _enemyModel;
    private float _lightsTimer;
    private float _oldSpeed;
    private float _maxStunTimer;
    public AudioSource _audioSource;
    public AudioClip _shootAudioClip;
    private bool _died;
    private QuestsMonitor _questsMonitor;
    private float _resetColorTime = 0.075f;
    public float _currentResetColorTime;
    public List<Material> _childMaterials = new List<Material>();
    public Transform[] _enemyChildrens;
    [Header("Flying enemy------")] 
    public float _wingsSpeed;
    
    [Header("------------------")] private float _refleshPlayerTargetcurrent;
    private float _reflashPlayerTargetMax = 1;

    public List<Transform> _wingsList = new List<Transform>(2);
    public float _baseSpeed;
    public bool _isPoweredUp;
    public float _poweredUpMultipler;
    public Rigidbody _rigidbody;
    private List<MeshRenderer> _meshRenderers = new List<MeshRenderer>();
    private bool _materialChanged = false;
    protected void SetUp()
    {
        //_rigidbody = GetComponent<Rigidbody>();
        _currentResetColorTime = _resetColorTime;
        _questsMonitor = FindFirstObjectByType<QuestsMonitor>();
        _gameManager = FindFirstObjectByType<GameManager>();
        int _poweredRandom = Random.Range(0, 100);
        if (_poweredRandom < _gameManager.GetPoweredEnemyChance())
        {
            _isPoweredUp = true;
            transform.localScale *= _poweredUpMultipler;
            _attackRange = (int)Math.Round(_attackRange * _poweredUpMultipler);
            _bulletDamage = (int)Math.Round(_bulletDamage * _poweredUpMultipler);
            _health *= _poweredUpMultipler;
        }
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance =_stoppingDistance;
        _audioSource = GetComponent<AudioSource>();
        _cameraShake = FindFirstObjectByType<CameraShake>();
        _enemyModel = transform.GetChild(transform.childCount - 1).gameObject;
        //_explosionPrefab = _gameManager._explosion;
        //_hitMaterial = _gameManager._hitMaterial;
        //_blackMaterial = _gameManager._blackMaterial;
        _baseSpeed = _agent.speed;
        _attackDelayCurrent = _attackDelayMax;
        if (_enemyModel == null) Debug.LogWarning("EMPTY ENEMY MODEL");
        
        foreach (var _child in transform.GetComponentsInChildren<Transform>())
             if (_child.GetComponent<MeshRenderer>())
                {
                    var _meshRenderer = _child.GetComponent<MeshRenderer>();
                    var _renderer = _meshRenderer;
                    _meshRenderers.Add(_meshRenderer);
                    _childMaterials.Add(_renderer.material);
                }
        _enemyChildrens = transform.GetComponentsInChildren<Transform>();

        _refleshPlayerTargetcurrent = _reflashPlayerTargetMax;
        _maxHealth = _health;
    }

    private void EnemyDie()
    {
        if(_died) return;
        _died = true;
        if(gameObject.GetComponent<Truck>())
        {
            Truck _truck = gameObject.GetComponent<Truck>();
            Collider[] _colliders = Physics.OverlapSphere(transform.position, _attackRange);

            foreach (Collider _collider in _colliders)
            {
                if (_collider.gameObject.GetComponent<Enemy>())
                {
                    Enemy _enemy = _collider.gameObject.GetComponent<Enemy>();
                    _enemy.CheckHealth(_bulletDamage);
                }
            }
        }
        
        _gameManager.RemoveEnemy(gameObject);
        
        if (!_killedByManager)
        {
            _gameManager._killedEnemies++;
            _player.AddPlayerXP(_xpReward);
            //_player._xp += _xpReward;
            _cameraShake.DoShake(.15f, .2f);
            DestroyClone(Vector3.zero);
            
            var _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            //_gameManager._gameSettings._boomPartiles.Add(_explosion.GetComponent<ParticleSystem>());   
            Destroy(_explosion, 5);
        }
        
        _questsMonitor._killedEnemies++;
        
        Destroy(gameObject);
    }

    public void DestroyClone(Vector3 _lastVelocity)
    {
        if (!_gameManager._gameSettings._qualityOn) return;

        var _meshFilter = GetComponent<MeshFilter>();
        var _mesh = _meshFilter.mesh;
        var _trash = Instantiate(_enemyModel, transform.position, transform.rotation);
        ChangeMeshColors(_trash.transform);
        _trash.name = $"{transform.name} :trash clone";
        _trash.AddComponent<Trash>();
        _trash.AddComponent<Rigidbody>();
        _trash.AddComponent<MeshCollider>();
        _trash.GetComponent<MeshCollider>().sharedMesh = _mesh;
        _trash.GetComponent<MeshCollider>().convex = true;
        if (_enemyType == EnemyType.Ground)
        {
            Vector3 _randomForceVector = new Vector3(Random.Range(-1, 1),
                Random.Range(5, 8),
                Random.Range(-1, 1));
            //_randomForceVector += _lastVelocity * 10;            NEED TO FIX THIS IN UPDATE 
            
            _trash.GetComponent<Rigidbody>().AddForce(_randomForceVector, ForceMode.Impulse);
            _trash.GetComponent<Rigidbody>().AddTorque(transform.up * _gameManager.GetTrashX());
            _trash.GetComponent<Rigidbody>().AddTorque(transform.right * _gameManager.GetTrashY());    
        }
        else if(_enemyType == EnemyType.Air)
        {
           _trash.GetComponent<Rigidbody>().AddTorque(_trash.transform.up * 50);
        }
        //_trash.isStatic = true;
        _trash.tag = "Trash";
    }

    private void ChangeMeshColors(Transform _parent)
    {
        foreach (var _child in _parent.GetComponentsInChildren<Transform>())
            if (_child.GetComponent<MeshRenderer>())
            {
                MeshRenderer _meshRenderer = _child.GetComponent<MeshRenderer>();
                HitChangeMaterial(_meshRenderer);
            }
    }

    protected void LookForColorChange()
    {
        _currentResetColorTime += Time.deltaTime;

        if (_currentResetColorTime > _resetColorTime)
        {
            if (!_materialChanged) return;
            short _index = 0;
            foreach (var _child in _enemyChildrens)
                if (_child.GetComponent<MeshRenderer>())
                {
                    _meshRenderers[_index].material = _childMaterials[_index];
                    _index++;
                }
            _materialChanged = false;
        }
        else
        {
            if (_materialChanged) return;
            short _index = 0;
            foreach (var _child in _enemyChildrens)
                if (_child.GetComponent<MeshRenderer>())
                {
                    _meshRenderers[_index].material = _hitMaterial;
                    _index++;
                }
            _materialChanged = true;
        }
    }
    public bool CheckHealth(float _value)
    {
        _health -= _value;
        if (!(_health <= 0)) return true;
        EnemyDie();
        return false;
    }

    protected void SwitchSpeed()
    {
        if (PlayerDistance() < 80)
        {
            _agent.speed = _baseSpeed;
        }
        else
        {
            _agent.speed = _baseSpeed * 4;
        }
    }

    protected float PlayerDistance()
    {
        return Vector3.Distance(transform.position, _player.transform.position);
    }

    protected void CheckStunned()
    {
        if(!_stunned) return;
        
        if (_maxStunTimer > _maxStunTime)
        {
            _stunned = false;
            _maxStunTimer = 0;
        }
        else
        {
            _maxStunTimer += Time.deltaTime;
        }
    }

    protected void Attack(Transform _bulletSpawn,int _bulletSpeed)
    {
        _bulletSpawn.LookAt(_player.transform.position);
        var _bulletInstance = Instantiate(_bullet, _bulletSpawn.position,
            _bulletSpawn.rotation);
        var _bulletScript = _bulletInstance.GetComponent<Bullet>();
        if (_bulletSpeed != 0)
        {
            _bulletScript._bulletSpeed = _bulletSpeed;
        }
        _bulletScript._enemyShoot = true;
        _bulletScript._bulletDamage = _bulletDamage;
        _audioSource.PlayOneShot(_shootAudioClip);
        Destroy(_bulletInstance, 5);
    }
    /*
    private IEnumerator HitChangeMaterial(Renderer _enemyPart, float _waitTime)
    {
        var _renderer = _enemyPart;
        Material _childOryginalMaterial = _renderer.material;
        if (_waitTime == 0)
            _renderer.material = _blackMaterial;
        else
            _renderer.material = _hitMaterial;
        yield return new WaitForSeconds(_waitTime);
        _renderer.material = _childOryginalMaterial;
    }
    */
    private void HitChangeMaterial(Renderer _enemyPart)
    {
        var _renderer = _enemyPart;
        _renderer.material = _blackMaterial;
    }
    protected void DoWings()
    {
        foreach (Transform _wing in _wingsList)
        {
            _wing.Rotate(new Vector3(0,0,1) * _wingsSpeed * Time.deltaTime, _wingsSpeed);
        }
    }

    protected void SetPlayerTarget()
    {
        if(_gameManager._player._died) return;
        if (_refleshPlayerTargetcurrent < _reflashPlayerTargetMax)
        {
            _refleshPlayerTargetcurrent += Time.deltaTime;
        }
        else
        {
            _agent.SetDestination(_player.transform.position);
            _refleshPlayerTargetcurrent = 0;
        }
    }
}