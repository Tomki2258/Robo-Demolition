using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent _agent;
    public GameManager _gameManager;
    public PlayerMovement _player;
    public GameObject _explosionPrefab;
    public int _attackRange;
    public int _incomingRange;
    public int _xpReward;
    public GameObject _bullet;
    public int _bulletDamage;
    public float health;
    public float _attackDelayMax;
    public float _attackDelayCurrent;
    public bool _stunned;
    private readonly int _stunTime = 5;
    private CameraShake _cameraShake;
    private float _lightsTimer;
    private float _oldSpeed;
    private float _stunTimer;
    public Material _oryginalMaterial;
    public Material _hitMaterial;
    public bool _killedByManager;
    public Material _blackMaterial;
    private GameObject _enemyModel;
    public Mesh _dieMesh;
    public void SetUp()
    {
        _agent = GetComponent<NavMeshAgent>();
        _oldSpeed = _agent.speed;
        _oryginalMaterial = GetComponent<Renderer>().material;
        _cameraShake = FindFirstObjectByType<CameraShake>();
        _enemyModel = transform.GetChild(transform.childCount - 1).gameObject;
        
        if(_enemyModel == null) Debug.LogWarning("EMPTY ENEMY MODEL");
    }
    public void EnemyDie()
    {
        _gameManager.RemoveEnemy(gameObject);
        if(!_killedByManager)
        {
            _gameManager._killedEnemies++;
            _player._xp += _xpReward;
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
        GameObject _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        //_gameManager._gameSettings._boomPartiles.Add(_explosion.GetComponent<ParticleSystem>());   
        Destroy(_explosion,5);
        Destroy(gameObject);
    }

    public void DestroyClone()
    {
        if(!_gameManager._gameSettings._qualityOn) return;
        
        Mesh _mesh = new Mesh();
        MeshFilter _meshFilter = GetComponent<MeshFilter>();
        _mesh = _meshFilter.mesh;
        GameObject _trash = Instantiate(_enemyModel,transform.position, transform.rotation);
        ChangeMeshColors(_trash.transform,0);
        _trash.name = $"{transform.name} :trash clone";
        _trash.AddComponent<Trash>();
        _trash.isStatic = true;
        _trash.tag = "Trash";
    }

    private void ChangeMeshColors(Transform _parent,float _waitTime)
    {
        foreach (Transform _child in _parent.GetComponentsInChildren<Transform>())
        {
            //Debug.LogWarning(_child.name);
            if (_child.GetComponent<MeshRenderer>())
            {
                MeshRenderer _meshRenderer = _child.GetComponent<MeshRenderer>();
                StartCoroutine(HitChangeMaterial(_meshRenderer,_waitTime));
            }
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
        
        ChangeMeshColors(transform,0.05f);
        
        return true;
    }

    public float PlayerDistance()
    {
        return Vector3.Distance(transform.position, _player.transform.position);
    }

    public void CheckStunned()
    {
        if (_stunTimer > _stunTime)
        {
            _stunned = false;
            _stunTimer = 0;
        }
        else
        {
            _stunTimer += Time.deltaTime;
        }

        if (_stunned)
            _agent.speed = 0;
        else
            _agent.speed = _oldSpeed;
    }

    public void Attack(Transform _bulletSpawn)
    {
        _bulletSpawn.LookAt(_player.transform.position);
        var _bulletInstance = Instantiate(_bullet, _bulletSpawn.position,
            _bulletSpawn.rotation);
        var _bulletScript = _bulletInstance.GetComponent<Bullet>();
        _bulletScript._enemyShoot = true;
        _bulletScript._bulletDamage = _bulletDamage;
        Destroy(_bulletInstance, 5);
    }

    private IEnumerator HitChangeMaterial(Renderer _enemyPart, float _waitTime)
    {
        Material _childOryginalMaterial;
        var _renderer = _enemyPart;
        _childOryginalMaterial = _renderer.material;
        if (_waitTime == 0)
        {
            _renderer.material = _blackMaterial;
        }
        else
        {
            _renderer.material = _hitMaterial;
        }
        yield return new WaitForSeconds(_waitTime);
        _renderer.material = _childOryginalMaterial;
    }
}