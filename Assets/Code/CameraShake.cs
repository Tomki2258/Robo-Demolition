using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    private IEnumerator _cameraEnumerator;
    [SerializeField] private bool _canShake;

    public void SwitchShakeMode(bool _mode)
    {
        _canShake = _mode;
    }
    public void DoShake(float duration, float magnitude)
    {
        _cameraEnumerator = Shake(duration, magnitude);
        StartCoroutine(_cameraEnumerator);
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        var originalPos = transform.localPosition;
        var elapsed = 0f;
        while (elapsed < duration)
        {
            if(!_canShake) yield break;
            
            var _xRandom = Random.Range(-1f, 1f) * magnitude;
            var _yRandom = Random.Range(-1f, 1f) * magnitude;
            transform.position = new Vector3(transform.position.x + _xRandom, transform.position.y + _yRandom,
                originalPos.z);
            elapsed += Time.deltaTime;

            yield return null;
        }
    }

    public void CancelShake()
    {
        StopCoroutine(_cameraEnumerator);
    }
}