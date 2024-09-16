using UnityEngine;

public class ExplosiveBullet : Bullet
{
    [Header("Child spript")] public int _explosionRange;
    public GameObject _explosionEffect;
    public int _granadeSpeed;

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
                if (_enemy.CheckHealth(_bulletDamage))
                {
                }

                DoDamage(other.transform.position);
                DoBulletEffect();
                Destroy(gameObject);
            }
            else
            {
                DoBulletEffect();
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
                //StartCoroutine(HitChangeMaterial(other.gameObject));
            }
            else
            {
                DoBulletEffect();
                Destroy(gameObject);
            }
        }
    }

    private void DoDamage(Vector3 _otherPositon)
    {
        Debug.LogWarning("Explosion bullet");
        var _colliders = Physics.OverlapSphere(_otherPositon, _explosionRange);

        foreach (var _col in _colliders)
            if (_col.GetComponent<Enemy>())
            {
                var _enemy = _col.GetComponent<Enemy>();
                _enemy.CheckHealth(_bulletDamage * 0.75f);
            }
    }

    private void DoBulletEffect()
    {
        var _explosionn = Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        Destroy(_explosionn, 1);
    }
}