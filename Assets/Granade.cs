using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    [Header("Child spript")] public int _explosionRange;
    public GameObject _explosionEffect;
    public int _granadeSpeed;
    public float _bulletDamage;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) return;
        
        DoDamage(transform.position);
        DoBulletEffect();
        Destroy(gameObject);
    }

    private void DoDamage(Vector3 _otherPositon)
    {
        Debug.LogWarning("Explosion bullet");
        var _colliders = Physics.OverlapSphere(_otherPositon, _explosionRange);

        foreach (var _col in _colliders)
            if (_col.GetComponent<Enemy>())
            {
                var _enemy = _col.GetComponent<Enemy>();
                _enemy.CheckHealth(_bulletDamage * 0.6f);
            }
    }

    private void DoBulletEffect()
    {
        var _explosionn = Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        _explosionn.transform.localScale = new Vector3(_explosionRange, _explosionRange, _explosionRange);
        Destroy(_explosionn, 1);
    }
}
