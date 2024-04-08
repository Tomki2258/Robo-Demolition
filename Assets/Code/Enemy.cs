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

    public void EnemyDie()
    {
        DestroyClone();
        _gameManager.RemoveEnemy(gameObject);
        if(!_killedByManager)
        {
            _gameManager._killedEnemies++;
            _player._xp += _xpReward;
            _cameraShake.DoShake(.15f, .2f);
            DestroyClone();
        }

        if (_gameManager._gameSettings._qualityOn)
        {
            GameObject _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            //_gameManager._gameSettings._boomPartiles.Add(_explosion.GetComponent<ParticleSystem>());   
            Destroy(_explosion,5);
        }

        _cameraShake.DoShake(0.001f, 1);
        
        Destroy(gameObject);
    }

    public void DestroyClone()
    {
        if(!_gameManager._gameSettings._qualityOn) return;
        
        Mesh _mesh = new Mesh();
        MeshFilter _meshFilter = GetComponent<MeshFilter>();
        _mesh = _meshFilter.mesh;
        
        
        GameObject _gameObject = new GameObject();
        _gameObject.AddComponent<MeshFilter>();
        _gameObject.AddComponent<MeshRenderer>();
        _gameObject.GetComponent<MeshFilter>().mesh = _mesh;
        _gameObject.GetComponent<MeshRenderer>().material = _blackMaterial;
        _gameObject.transform.position = transform.position;
        _gameObject.isStatic = true;
        _gameObject.tag = "Trash";
        int _x = Random.Range(0, 360);
        int _y = Random.Range(0, 360);
        _gameManager.transform.rotation 
            = Quaternion.Euler(_x, _y, 0);
    }
    public void SetUp()
    {
        _agent = GetComponent<NavMeshAgent>();
        _oldSpeed = _agent.speed;
        _oryginalMaterial = GetComponent<Renderer>().material;
        _cameraShake = FindFirstObjectByType<CameraShake>();
    }

    public bool CheckHealth(float _value)
    {
        health -= _value;
        if (health <= 0)
        {
            EnemyDie();
            return false;
        }

        StartCoroutine(HitChangeMaterial());
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
    private IEnumerator HitChangeMaterial()
    {
        var _renderer = GetComponent<Renderer>();
        _renderer.material = _hitMaterial;
        yield return new WaitForSeconds(0.05f);
        _renderer.material = _oryginalMaterial;
    }
}