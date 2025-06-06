using UnityEngine;

public class RocketEnemy : Enemy
{
    [Header("Rocket enemy elements")] public GameObject _rocketPrefab;

    public GameObject _rocketTargetPrefab;
    public Transform _shootTransform;
    [SerializeField] private int _rocketDamage;
    private void Start()
    {
        SetUp();
    }

    private void FixedUpdate()
    {
        if (_player._died) return;
        SetPlayerTarget();
        SwitchSpeed();
        if (Vector3.Distance(transform.position, _player.transform.position) < _attackRange)
        {
            if (_attackDelayCurrent > _attackDelayMax)
            {
                var _currentRocketTarget = Instantiate(_rocketTargetPrefab,
                    _player.transform.position,
                    Quaternion.identity);
            

                _attackDelayCurrent = 0;

                var _currentRocket = Instantiate(_rocketPrefab,
                    _shootTransform.position,
                    Quaternion.identity);
                var _currentRocketComponent = _currentRocket.GetComponent<Rocket>();
                _currentRocketComponent._rocketTargetPrefab = _currentRocketTarget;
                _currentRocketComponent._isEnemy = true;
                _currentRocketComponent._rocketDamage = _rocketDamage;
                _currentRocketComponent._enemy = _currentRocketTarget.transform;
                _audioSource.PlayOneShot(_shootAudioClip);
                //shoot
            }
            else
            {
                _attackDelayCurrent += Time.deltaTime;
            }
        }

        CheckStunned();
    }
}