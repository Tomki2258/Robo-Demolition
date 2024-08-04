using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : Enemy
{
    private float _currentPlayTime;
    public float _maxPlayTime;
    void Start()
    {
        SetUp();
    }

    void FixedUpdate()
    {
        if(_player._died) return;
        
        var _distance = PlayerDistance();
        SetPlayerTarget();
        SwitchSpeed();
        CheckStunned();
        ManageRandomAudio();
        if (_distance < _attackRange)
        {
            Attacking();
        }
    }
    private void ManageRandomAudio()
    {
        if (_currentPlayTime > _maxPlayTime)
        {
            float _random = Random.Range(0, 3.5f);
            StartCoroutine(RandomAudioPlay(_random));
            _currentPlayTime = 0;
        }
        else
        {
            _currentPlayTime += Time.deltaTime;
        }
    }
    private void Attacking()
    {
        GameObject _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _explosion.transform.localScale = new Vector3(2, 2, 2);
        
        _player.CheckHealth(_bulletDamage);
        
        DestroyClone();
        Destroy(gameObject);
    }

    private IEnumerator RandomAudioPlay(float _playTime)
    {
        _audioSource.Play();
        yield return new WaitForSeconds(_playTime);
        _audioSource.Stop();
    }
}
