using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 _offset;
    public float _speed;
    public Transform _target;
    public float _rotationSpeed;
    private void Update()
    {
        var _desiredPosition = _target.position + (_offset * _target.localScale.x);
        var _smoothedPosition = Vector3.Lerp(transform.position, _desiredPosition, _speed * Time.deltaTime);
        transform.position = _smoothedPosition;
        
        Vector3 lookDirection =  _target.position - transform.position;
        lookDirection.Normalize();

        transform.rotation
            = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), _rotationSpeed * Time.deltaTime);
    }
}