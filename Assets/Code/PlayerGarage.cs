using System.Collections.Generic;
using UnityEngine;

public class PlayerGarage : MonoBehaviour
{
    [SerializeField]private List<Transform> _rotateElements;
    [SerializeField] private int _rotateSpeedScale;
    private bool _garageEnabled = false;
    void FixedUpdate()
    {
        if(!_garageEnabled) return;
        RotateElements();
    }

    private void RotateElements()
    {
        foreach (Transform _element in _rotateElements)
        {
            _element.Rotate(0, _rotateSpeedScale * Time.deltaTime, 0);
        }
    }
}
