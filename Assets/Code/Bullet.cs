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
        Destroy(gameObject, 6);
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

    private void HitFunction(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet")) return;
        
        //Is bullet shot by player
        if (!_enemyShoot)
        {
            switch (other.gameObject.tag)
            {
                case "Player":
                    break;
                case "Enemy":
                    var _enemy = other.GetComponent<Enemy>();
                    if (_enemy.CheckHealth(_bulletDamage))
                    {
                        _enemy._currentResetColorTime = 0;
                    }
                    Destroy(gameObject);
                    break;
                case "Turret":
                    var _turret = other.GetComponent<Turret>();
                    if (_turret.CheckHealth(_bulletDamage))
                    {
                        //_enemy._currentResetColorTime = 0;
                    }
                    Destroy(gameObject);
                    break;
                default:
                    Destroy(gameObject);
                    break;
            }
        }
        else //Is bullet shot by enemy
        {
            switch (other.gameObject.tag)
            {
                case "Player":
                    var _player = other.GetComponent<PlayerMovement>();
                    _player.CheckHealth(_bulletDamage);
                    Destroy(gameObject);
                    break;
                case "Enemy":
                    break;
                default:
                    Destroy(gameObject);
                    break;
            }
        }
    }
}