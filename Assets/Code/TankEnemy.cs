using UnityEngine;

public class TankEnemy : Enemy
{
    public Transform _shootingPoint;
    public Transform _topModel;

    private void Start()
    {
        SetUp();
    }

    private void FixedUpdate()
    {
        if (_player._died) return;
        SetPlayerTarget();
        Attacking();
        CheckStunned();
        SwitchSpeed();
        //_topModel.LookAt(_player.transform.position);
        // Vector3 lookDirection = new Vector3(
        //     _topModel.position.x
        //     ,(_player.transform.position.y - _topModel.position.y)
        //     ,_topModel.position.z);
        // lookDirection.Normalize();
        //
        // _topModel.rotation = Quaternion.Slerp(_topModel.rotation, Quaternion.LookRotation(lookDirection), 1 * Time.deltaTime);
    }

    public void Attacking()
    {
        if (_stunned) return;

        var _distance = PlayerDistance();
        if (_distance > _attackRange) return;
        Debug.Log("Attacking !");
        if (_attackDelayCurrent > _attackDelayMax)
        {
            _attackDelayCurrent = 0;
            Attack(_shootingPoint);
            //shoot
        }
        else
        {
            _attackDelayCurrent += Time.deltaTime;
        }
    }
}