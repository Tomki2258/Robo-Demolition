using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _bulletSpeed;
    public float _bulletDamage = 5;
    public bool _enemyShoot;
    public Material _redMaterial;
    private Material _lastEnemyMaterial;
    public Vector3 _flyVector = Vector3.forward;
    private void Start()
    {
        Destroy(gameObject, 5);
    }
    public void AddRecoil(float _recoil)
    {
        float value = Random.Range(-_recoil, _recoil) / 10f;
        _flyVector = new Vector3(Random.Range(-value,value),0, 1);
    }
    private void Update()
    {
        transform.position += transform.TransformDirection(_flyVector * _bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        HitFunction(other);
    }

    public void HitFunction(Collider other)
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
                if (_enemy.CheckHealth(_bulletDamage))
                {
                }

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
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag("Enemy"))
            {
                //StartCoroutine(HitChangeMaterial(other.gameObject));
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}