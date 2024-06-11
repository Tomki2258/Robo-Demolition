using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    private Renderer _renderer;
    private float _height;
    private Vector3 _collapseVector;
    public float _fallingSpeed;
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _height = _renderer.bounds.size.y;

        _collapseVector = new Vector3(
            transform.position.x,
            transform.position.y - _height,
            transform.position.z);
    }

    public Vector3 GetCollabseVector()
    {
        return _collapseVector;
    }
}
