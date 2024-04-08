using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    public void DoShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        var originalPos = transform.localPosition;
        var elapsed = 0f;
        while (elapsed < duration)
        {
            var _xRandom = Random.Range(-1f, 1f) * magnitude;
            var _yRandom = Random.Range(-1f, 1f) * magnitude;
            transform.position = new Vector3(transform.position.x + _xRandom, transform.position.y + _yRandom, originalPos.z);
            elapsed += Time.deltaTime;

            yield return null;
        }
    }
}