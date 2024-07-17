using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum destoryableType
{
    NONE,
    Falling
}
public class Destroyable : MonoBehaviour
{
    public float _fallingSpeed;
    public Vector3 _collapseVector;
    private float _height;
    private Renderer _renderer;
    public destoryableType _type;
    private Vector3 _startRotation;
    private float _startXSize;
    private float tolerance = 5f;
    private AudioSource _audioSource;
    public AudioClip _fallingSound;
    public AudioClip _secretSound;
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _height = _renderer.bounds.size.y;
        _audioSource = GetComponent<AudioSource>();
        if(_type == destoryableType.NONE)
        {
            _collapseVector = new Vector3(
                transform.position.x,
                transform.position.y - _height,
                transform.position.z);
        }
        else if(_type == destoryableType.Falling)
        {
            _startRotation = transform.localEulerAngles;
            _collapseVector = new Vector3(
                transform.position.x + Mathf.Sin(Time.time * 50) * 0.03f,
                transform.position.y,
                transform.position.z + +Mathf.Sin(Time.time * 50) * 0.03f);
        }
    }

    public void PlayFallingSound()
    {
        int value = Random.Range(0, 100);
        if(value < 2)
        {
            _audioSource.PlayOneShot(_secretSound);
            return;
        }
        _audioSource.PlayOneShot(_fallingSound);
    }
    public Vector3 GetCollabseVector()
    {
        return _collapseVector;
    }
    public bool CheckHorizontal()
    {
        Vector3 rotation = transform.eulerAngles;
        //_fallingSpeed += Time.deltaTime * 2;
        bool isHorizontal = (Mathf.Abs(rotation.x - 90) < tolerance || Mathf.Abs(rotation.x - 270) < tolerance ||
                             Mathf.Abs(rotation.z - 90) < tolerance || Mathf.Abs(rotation.z - 270) < tolerance);
        
        return isHorizontal;
    }
}