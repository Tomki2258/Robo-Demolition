using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : MonoBehaviour
{
    private Transform _wings;
    private float _randomMultipler;
    void Start()
    {
        _wings = transform.GetChild(0);
        _randomMultipler = Random.Range(0, 360);
        _wings.transform.Rotate(0,0,_randomMultipler);
    }

    void FixedUpdate()
    {
        _wings.transform.Rotate(0,0,90 * Time.deltaTime);
    }
}
