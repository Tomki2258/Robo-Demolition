using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _bulletSpeed;
    public int _bulletDamage = 5;
    public bool _enemyShoot;

    private void Start()
    {
        Destroy(gameObject, 5);
    }

    private void Update()
    {
        transform.position += transform.TransformDirection(Vector3.forward * _bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet")) return;

        if (!_enemyShoot)
        {
            if (other.gameObject.CompareTag("Player"))
            {
            }
            else if (other.gameObject.CompareTag("Enemy"))
            {
                var _enemy = other.GetComponent<Enemy>();
                _enemy.CheckHealth(_bulletDamage);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var _player = other.GetComponent<PlayerMovement>();
                _player.CheckHealth(_bulletDamage);
            }
            else if (other.gameObject.CompareTag("Enemy"))
            {
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}