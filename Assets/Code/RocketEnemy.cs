using UnityEngine;

public class RocketEnemy : Enemy
{
    [Header("Rocket enemy elements")] public GameObject _rocketPrefab;

    public GameObject _rocketTargetPrefab;
    public Transform _shootTransform;

    private void Start()
    {
        SetUp();
    }

    private void FixedUpdate()
    {
        if (_player._died) return;

        _agent.SetDestination(_player.transform.position);

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
                _currentRocketComponent._isEnemy = true;
                _currentRocketComponent._enemy = _currentRocketTarget.transform;
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