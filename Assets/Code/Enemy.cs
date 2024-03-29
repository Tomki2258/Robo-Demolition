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

    private void OnDestroy()
    {
        _gameManager.RemoveEnemy(gameObject);
        _gameManager._killedEnemies++;
        //GameObject _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        //_cameraShake.DoShake(0.001f, 1);
        _player._xp += _xpReward;
        //Destroy(_explosion,5);
    }

    public void SetUp()
    {
        _agent = GetComponent<NavMeshAgent>();
        _oldSpeed = _agent.speed;
    }

    public bool CheckHealth(float _value)
    {
        health -= _value;
        if (health <= 0)
        {
            Destroy(gameObject);
            return false;
        }

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
}