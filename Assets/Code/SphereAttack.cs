using UnityEngine;

public class SphereAttack : MonoBehaviour
{
    public float _playerScale;
    public PlayerMovement _player;
    public int _damage;
    public float _maxScale;
    public float _speed;

    private void Start()
    {
        if (_player._died) Destroy(gameObject);

        _playerScale = _player.transform.localScale.x;
        _maxScale *= _playerScale;
        _speed *= _playerScale;
    }

    private void FixedUpdate()
    {
        var _scaleX = transform.localScale.x;
        if (_scaleX < _maxScale)
            transform.localScale += new Vector3(_speed, _speed, _speed) * Time.deltaTime;
        else
            Destroy(gameObject);

        transform.position = _player.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) other.GetComponent<Enemy>()._stunned = true;
        //Debug.LogWarning($"{other.name} is stunned !");
    }
}