using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    private Renderer _renderer;
    private float _height;
    private Vector3 _collapseVector;
    private bool _canCollapse;
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

    private void FixedUpdate()
    {
        if(!_canCollapse) return;

        if (Vector3.Distance(transform.position,_collapseVector) > 0.2f)
        {
            transform.position = Vector3.Lerp(
                new Vector3(
                     transform.position.x + Mathf.Sin(Time.time * 50 ) * 0.03f,
                    transform.position.y,
                    transform.position.z +  + Mathf.Sin(Time.time * 50 ) * 0.03f), _collapseVector, _fallingSpeed * Time.deltaTime);
        }
        else
        {
            
        }
    }

    public void DoCollapse()
    {
        _canCollapse = true;
    }
}
