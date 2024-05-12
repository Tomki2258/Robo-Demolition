using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemolition : MonoBehaviour
{
    public Renderer _playerRenderer;
    public float _playerHeight = 0;
    
    public void UpdatePlayerSize()
    {
        _playerHeight = _playerRenderer.bounds.size.y;
    }
    private bool CheckDestroy(GameObject _object)
    {
        Renderer _objectRenderer = _object.GetComponent<Renderer>();
        float sizeY = _objectRenderer.bounds.size.y;
        if (sizeY < _playerHeight) return true;
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Destroyable")) return;
        
        if (CheckDestroy(other.gameObject))
        {
            Destroy(other.gameObject);
        }
    }
}
