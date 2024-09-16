using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Transform _enemy;
    public float _rocketSpeed;
    public GameObject _explosionFX;
    public float _rocketDamage;
    public float _rocketRange;
    public bool _isEnemy;
    private CameraShake _cameraShake;
    private GameObject _currentFX;
    private GameManager _gameManager;
    private Vector3 _lastKnownPosition;
    private PlayerMovement _player;
    public bool _starterDone;
    private Vector3 _startTarget;
    public GameObject _rocketTargetPrefab;

    private void Start()
    {
        _cameraShake = FindAnyObjectByType<CameraShake>();
        _gameManager = FindAnyObjectByType<GameManager>();
        _startTarget = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
        _player = _gameManager._player;
    }

    private void FixedUpdate()
    {
        if (!_starterDone)
        {
            transform.position = Vector3.MoveTowards(transform.position, _startTarget, _rocketSpeed * Time.deltaTime);
            RotateToTarget(_startTarget);
            if (GetDistance(_startTarget) < 0.1f)
                _starterDone = true;
            return;
        }

        if (!_isEnemy)
            if(_enemy == null)
                _enemy = GetNearestEnemy();
        
        if (_enemy != null) _lastKnownPosition = _enemy.position;
        transform.position = Vector3.MoveTowards(transform.position, _lastKnownPosition,
            _rocketSpeed * Time.deltaTime);
        _rocketSpeed *= 1.01f;
        RotateToTarget(_lastKnownPosition);
        
        if (GetDistance(_lastKnownPosition) < 0.1f)
        {
            _currentFX = Instantiate(_explosionFX, transform.position, Quaternion.identity);
            DoDamage();
            Destroy(gameObject);
            if(_rocketTargetPrefab != null) Destroy(_rocketTargetPrefab);
        }
    }

    private void RotateToTarget(Vector3 _target)
    {
        var _direction = _target - transform.position;
        _direction.Normalize();
        transform.rotation =
            Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), 10 * Time.deltaTime);
    }

    private float GetDistance(Vector3 _target)
    {
        return Vector3.Distance(transform.position, _target);
    }

    private Transform GetNearestEnemy()
    {
        Transform _currentEnemy = null;
        var lowestDist = Mathf.Infinity;
        foreach (var _enemy in _gameManager._spawnedEnemies)
        {
            if (_enemy == null) continue;
            var dist = Vector3.Distance(_enemy.transform.position, transform.position);

            if (dist < lowestDist)
            {
                lowestDist = dist;
                _currentEnemy = _enemy.transform;
            }
        }

        return _currentEnemy;
    }

    private void DoDamage()
    {
        _cameraShake.DoShake(0.001f, 0.5f);

        var _colliders = Physics.OverlapSphere(transform.position, _rocketRange);
        
        if (_isEnemy)
        {
            float _playerRocketRange = _rocketRange / 2;

            float _rangeToPlayer = Vector3.Distance(_player.transform.position, transform.position);

            if (_rangeToPlayer < _playerRocketRange)
            {
                //Debug.LogWarning("Rocket hit");
                _player.CheckHealth(_rocketDamage);
            }
        }
        else
        {
             foreach (var _obj in _colliders)
             {
                 if (_obj.GetComponent<Enemy>())
                 {
                     //Debug.LogWarning("Enemy hit" + _obj.name);
                     if (!_obj.GetComponent<Enemy>().CheckHealth(_rocketDamage))
                     {
                         //_explosionFX.GetComponent<AudioSource>().enabled = false;
                     }
                 }
             } 
        }
    }
}