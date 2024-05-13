using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDemolition : MonoBehaviour
{
    public Renderer _playerRenderer;
    public float _playerHeight = 0;
    
    public void UpdatePlayerSize()
    {
        _playerHeight = _playerRenderer.bounds.size.y * 2;
    }

    private void FixedUpdate()
    {
        Collider[] _colliders = Physics.OverlapSphere(transform.position, 3 * transform.localScale.x);

        foreach (Collider _col in _colliders)
        {
            if (_col.CompareTag("Destroyable"))
            {
                if (CheckDestroy(_col.gameObject))
                {
                    _col.tag = "Destroyed";
                    _col.gameObject.isStatic = false;
                    _col.GetComponent<Destroyable>().DoCollapse();
                }
            }
        }
    }

    private bool CheckDestroy(GameObject _object)
    {
        Renderer _objectRenderer = _object.GetComponent<Renderer>();
        float sizeY = _objectRenderer.bounds.size.y;
        Debug.LogWarning($"Player heigjt {_playerHeight} / object {sizeY}");
        if (sizeY < _playerHeight) return true;
        return false;
    }
}
