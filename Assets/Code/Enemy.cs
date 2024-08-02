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
    public float health;
    public float _attackDelayMax;
    public float _attackDelayCurrent;
    public bool _stunned;
    public Material _oryginalMaterial;
    public Material _hitMaterial;
    public bool _killedByManager;
    public Material _blackMaterial;
    public Mesh _dieMesh;
    private readonly int _maxStunTime = 5;
    private CameraShake _cameraShake;
    private GameObject _enemyModel;
    private float _lightsTimer;
    private float _oldSpeed;
    private float _maxStunTimer;
    public AudioSource _audioSource;
    public AudioClip _shootAudioClip;
    private bool _died;
    [Header("Flying enemy------")] 
    public float _wingsSpeed;
    
    [Header("------------------")] private float _refleshPlayerTargetcurrent;
    private float _reflashPlayerTargetMax = 0.5f;

    public List<Transform> _wingsList = new List<Transform>(2);
    public float _baseSpeed;
    public bool _isPoweredUp;
    public float _poweredUpMultipler;
    public void SetUp()
    {
        _gameManager = FindFirstObjectByType<GameManager>();
        int _poweredRandom = Random.Range(0, 100);
        if (_poweredRandom < _gameManager.GetPoweredEnemyChance())
        {
            _isPoweredUp = true;
            transform.localScale *= _poweredUpMultipler;
            _attackRange = (int)Math.Round(_attackRange * _poweredUpMultipler);
            _bulletDamage = (int)Math.Round(_bulletDamage * _poweredUpMultipler);
            health *= _poweredUpMultipler;
        }
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance =_stoppingDistance;
        _audioSource = GetComponent<AudioSource>();
        _oryginalMaterial = GetComponent<Renderer>().material;
        _cameraShake = FindFirstObjectByType<CameraShake>();
        _enemyModel = transform.GetChild(transform.childCount - 1).gameObject;
        //_explosionPrefab = _gameManager._explosion;
        //_hitMaterial = _gameManager._hitMaterial;
        //_blackMaterial = _gameManager._blackMaterial;
        _baseSpeed = _agent.speed;
        _attackDelayCurrent = _attackDelayMax;
        if (_enemyModel == null) Debug.LogWarning("EMPTY ENEMY MODEL");
    } 

    public void EnemyDie()
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
            DestroyClone();
        }

        /*
        if (_gameManager._gameSettings._qualityOn)
        {
            GameObject _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            //_gameManager._gameSettings._boomPartiles.Add(_explosion.GetComponent<ParticleSystem>());
            Destroy(_explosion,5);
        }
        */
        var _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        //_gameManager._gameSettings._boomPartiles.Add(_explosion.GetComponent<ParticleSystem>());   
        Destroy(_explosion, 5);
        Destroy(gameObject);
    }

    public void DestroyClone()
    {
        if (!_gameManager._gameSettings._qualityOn) return;

        var _mesh = new Mesh();
        var _meshFilter = GetComponent<MeshFilter>();
        _mesh = _meshFilter.mesh;
        var _trash = Instantiate(_enemyModel, transform.position, transform.rotation);
        ChangeMeshColors(_trash.transform, 0);
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

    private void ChangeMeshColors(Transform _parent, float _waitTime)
    {
        foreach (var _child in _parent.GetComponentsInChildren<Transform>())
            //Debug.LogWarning(_child.name);
            if (_child.GetComponent<MeshRenderer>())
            {
                var _meshRenderer = _child.GetComponent<MeshRenderer>();
                StartCoroutine(HitChangeMaterial(_meshRenderer, _waitTime));
            }
    }

    public bool CheckHealth(float _value)
    {
        //Debug.LogWarning("Enemy hit");
        health -= _value;
        if (health <= 0)
        {
            EnemyDie();
            return false;
        }

        ChangeMeshColors(transform, 0.05f);

        return true;
    }

    public void SwitchSpeed()
    {
        if (PlayerDistance() < 60)
        {
            _agent.speed = _baseSpeed;
        }
        else
        {
            _agent.speed = _baseSpeed * 4;
        }
    }
    public float PlayerDistance()
    {
        return Vector3.Distance(transform.position, _player.transform.position);
    }

    public void CheckStunned()
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
    
    public void Attack(Transform _bulletSpawn)
    {
        _bulletSpawn.LookAt(_player.transform.position);
        var _bulletInstance = Instantiate(_bullet, _bulletSpawn.position,
            _bulletSpawn.rotation);
        var _bulletScript = _bulletInstance.GetComponent<Bullet>();
        _bulletScript._enemyShoot = true;
        _bulletScript._bulletDamage = _bulletDamage;
        _audioSource.PlayOneShot(_shootAudioClip);
        Destroy(_bulletInstance, 5);
    }

    private IEnumerator HitChangeMaterial(Renderer _enemyPart, float _waitTime)
    {
        Material _childOryginalMaterial;
        var _renderer = _enemyPart;
        _childOryginalMaterial = _renderer.material;
        if (_waitTime == 0)
            _renderer.material = _blackMaterial;
        else
            _renderer.material = _hitMaterial;
        yield return new WaitForSeconds(_waitTime);
        _renderer.material = _childOryginalMaterial;
    }

    public void DoWings()
    {
        foreach (Transform _wing in _wingsList)
        {
            _wing.Rotate(new Vector3(0,0,1) * _wingsSpeed * Time.deltaTime, _wingsSpeed);
        }
    }

    public void SetPlayerTarget()
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