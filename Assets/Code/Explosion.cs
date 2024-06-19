using UnityEngine;

public class Explosion : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
        Destroy(gameObject, 3);
    }
}