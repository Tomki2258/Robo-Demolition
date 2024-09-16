using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Windmill : MonoBehaviour
{
    [SerializeField] private List<GameObject> _windmills;
    [SerializeField] private float _speed;
    [SerializeField] private List<GameObject> _blades;

    private void Start()
    {
        foreach (var windmill in _windmills)
        {
            var randomRotation = Random.Range(0, 360);
            var blade = windmill.transform.GetChild(0).gameObject;
            _blades.Add(blade);
            blade.transform.Rotate(Vector3.forward * randomRotation);
        }
    }

    private void FixedUpdate()
    {
        foreach (var windmill in _blades) 
            windmill.transform.Rotate(Vector3.forward * (_speed * Time.deltaTime));
    }
}