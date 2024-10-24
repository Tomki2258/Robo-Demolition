using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Windmill : MonoBehaviour
{
    [SerializeField] private List<GameObject> _windmills;
    [SerializeField] private float _speed;
    [SerializeField] private List<Transform> _blades;
    private int _bladesCount;
    private void Start()
    {
        foreach (var windmill in _windmills)
        {
            var randomRotation = Random.Range(0, 360);
            var blade = windmill.transform.GetChild(0).gameObject;
            _blades.Add(blade.transform);
            blade.transform.Rotate(Vector3.forward * randomRotation);
        }
        _bladesCount = _windmills.Count;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _bladesCount; i++)
        {
            _blades[i].Rotate(Vector3.forward * _speed);
        }
    }
}